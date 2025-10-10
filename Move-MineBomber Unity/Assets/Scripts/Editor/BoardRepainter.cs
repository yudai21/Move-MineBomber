using Bomb.Views;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(BoardViewer))]
public class BoardRepainter : Editor
{
    BoardViewer _viewer;
    private void OnEnable()
    {
        _viewer = (BoardViewer)target;
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
                _viewer.Repaint();
        });
        button.text = "描画リセット";
        root.Add(button);

        return root;
    }
}
