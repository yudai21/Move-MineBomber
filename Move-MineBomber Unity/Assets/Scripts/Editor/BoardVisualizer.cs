using Bomb.Managers;
using Bomb.Views;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(GameSceneRooter))]
public class BoardVisualizer : Editor
{
    GameSceneRooter _rooter;
    private void OnEnable()
    {
        _rooter = (GameSceneRooter)target;
    }
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
        var iterator = serializedObject.GetIterator();
        iterator.NextVisible(true);
        while (iterator.NextVisible(false))
        {
            if (iterator.name == "m_Script") continue; // Scriptフィールド非表示
            root.Add(new PropertyField(iterator.Copy()));
        }


        // カスタム要素を追加したい場合はここに書く
        var button = new Button(() =>
        {
            if (Application.isPlaying)
                Debug.Log(_rooter.Manager.Board.Board.ToString());
        });
        button.text = "現在のボードを出力";
        root.Add(button);

        return root;
    }
}
