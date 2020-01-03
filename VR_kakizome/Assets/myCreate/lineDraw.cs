using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineDraw : MonoBehaviour
{
    //SerializeFieldをつけるとInspectorウィンドウからゲームオブジェクトやPrefabを指定できる。
    [SerializeField] GameObject LineObjectPrefab;

     //現在描画中のLineObject;
    private GameObject CurrentLineObject = null;
     
     // コントローラーを設定
    private OVRInput.Controller controller;

    void Start()
    {
        // OVRControllerHelperから左右どっちかを取得する
        controller = GetComponent<OVRControllerHelper>().m_controller;
    }

    void Update()
    {
	//人差し指のトリガーが引かれている間
	if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger, controller)) {
      	if(CurrentLineObject == null){
                //PrefabからLineObjectを生成
                CurrentLineObject = Instantiate(LineObjectPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            	}
            	//ゲームオブジェクトからLineRendererコンポーネントを取得
            LineRenderer render = CurrentLineObject.GetComponent<LineRenderer>();

            //LineRendererからPositionsのサイズを取得
            int NextPositionIndex = render.positionCount;

            //LineRendererのPositionsのサイズを増やす
            render.positionCount = NextPositionIndex + 1;

            //LineRendererのPositionsに現在のコントローラーの位置情報を追加
            render.SetPosition(NextPositionIndex, transform.position);	
	}
      else if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger))//人差し指のトリガーを離したとき
        {
            if(CurrentLineObject != null)
            {
                //現在描画中の線があったらnullにして次の線を描けるようにする。
                CurrentLineObject = null;
            }
        }
    }
}
