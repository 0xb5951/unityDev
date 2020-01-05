using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hand_input : MonoBehaviour
{    
	private OVRHand ovrHand;
    //SerializeFieldをつけるとInspectorウィンドウからゲームオブジェクトやPrefabを指定できる。
    [SerializeField] GameObject LineObjectPrefab;
    [SerializeField] OVRSkeleton skeleton;

    private bool _isIndexPinching;
    private bool _isMiddlePinching;
    private bool _isRingPinching;
    private bool _isPinkyPinching;
    private int _pinchingFingerNumber;

      //生成したObjectを持っておくためのList
     List<GameObject> list_toggle_ = new List<GameObject>();

     //現在描画中のLineObject;
    private GameObject CurrentLineObject = null;

	void Start() {
    		ovrHand = GetComponent<OVRHand>();
	}


    void Update()
    {
        _isIndexPinching = ovrHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        _isMiddlePinching = ovrHand.GetFingerIsPinching(OVRHand.HandFinger.Middle);
        _isRingPinching = ovrHand.GetFingerIsPinching(OVRHand.HandFinger.Ring);
        _isPinkyPinching = ovrHand.GetFingerIsPinching(OVRHand.HandFinger.Pinky);

        _pinchingFingerNumber = 0;
        if (_isIndexPinching) _pinchingFingerNumber++;
        if (_isMiddlePinching) _pinchingFingerNumber++;
        if (_isRingPinching) _pinchingFingerNumber++;
        if (_isPinkyPinching) _pinchingFingerNumber++;
	
	// 人差し指の先端位置を取得
	Vector3 indexTipPos = skeleton.Bones[(int) OVRSkeleton.BoneId.Hand_IndexTip].Transform.position;

	if (_pinchingFingerNumber == 1) {
      	if(CurrentLineObject == null){
                //PrefabからLineObjectを生成
                CurrentLineObject = Instantiate(LineObjectPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		    //生成したインスタンスをリストで持っておく
        	   list_toggle_.Add(CurrentLineObject);
            	}
            	//ゲームオブジェクトからLineRendererコンポーネントを取得
            LineRenderer render = CurrentLineObject.GetComponent<LineRenderer>();

            //LineRendererからPositionsのサイズを取得
            int NextPositionIndex = render.positionCount;

            //LineRendererのPositionsのサイズを増やす
            render.positionCount = NextPositionIndex + 1;

            //LineRendererのPositionsに現在のコントローラーの位置情報を追加
            render.SetPosition(NextPositionIndex, indexTipPos);	
	}
      else
        {
            if(CurrentLineObject != null)
            {
                //現在描画中の線があったらnullにして次の線を描けるようにする。
                CurrentLineObject = null;
            }
        }		

    if (_pinchingFingerNumber > 2) {            
		//リストで保持しているインスタンスを削除
		for (int i = 0; i < list_toggle_.Count; i++)
		{
		    Destroy(list_toggle_[i]);
		}

		//リスト自体をキレイにする
		list_toggle_.Clear();
        }
    }
}
