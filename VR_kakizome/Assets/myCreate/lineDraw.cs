using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineDraw : MonoBehaviour
{
    //SerializeFieldをつけるとInspectorウィンドウからゲームオブジェクトやPrefabを指定できる。
    [SerializeField] GameObject LineObjectPrefab;
    [SerializeField] Transform HandAnchor;//positionを取得するコントローラーの位置情報

      //生成したObjectを持っておくためのList
     List<GameObject> list_toggle_ = new List<GameObject>();

     //現在描画中のLineObject;
    private GameObject CurrentLineObject = null;
     
     // コントローラーを設定
    private OVRInput.Controller controller;

    private Transform Pointer
    {
        get
        {
            return HandAnchor;
        }
    }

    void Start()
    {
        // OVRControllerHelperから左右どっちかを取得する
        controller = GetComponent<OVRControllerHelper>().m_controller;
    }

    void Update()
    {
	var pointer = Pointer;

	//人差し指のトリガーが引かれている間
	if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger, controller)) {
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
            render.SetPosition(NextPositionIndex, pointer.position);	
	}
      else if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger))//人差し指のトリガーを離したとき
        {
            if(CurrentLineObject != null)
            {
                //現在描画中の線があったらnullにして次の線を描けるようにする。
                CurrentLineObject = null;
            }
        }

    if (OVRInput.GetDown(OVRInput.Button.One, controller)) {            
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
