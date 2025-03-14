#if UNITY_EDITOR
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Dunward.Capricorn
{
    public abstract class BaseNode : Node
    {
        public readonly GraphView graphView;
        public readonly NodeMainContainer main;

        protected NodeType nodeType;

        protected int id;
        public int ID
        {
            get => id;
        }

        public BaseNode(GraphView graphView, int id, float x, float y)
        {
            this.graphView = graphView;
            this.id = id;

            titleContainer.AddToClassList("capricorn-title-container");
            mainContainer.Insert(1, extensionContainer);

            SetupTitleContainer();
            SetPosition(new Rect(x, y, 0, 0));

            main = new NodeMainContainer(this);

            RegisterCallback<FocusInEvent>(evt => graphView.Record());

            RefreshExpandedState();
        }

        public BaseNode(GraphView graphView, int id, Vector2 mousePosition) : this(graphView, id, mousePosition.x, mousePosition.y)
        {

        }

        public BaseNode(GraphView graphView, NodeMainData mainData) : this(graphView, mainData.id, mainData.x, mainData.y)
        {
            UpdateSubContainers(mainData);
        }

        public virtual NodeMainData GetMainData()
        {
            main.action.SerializeConnections();

            var mainData = new NodeMainData();
            mainData.id = id;
            mainData.x = GetPosition().x;
            mainData.y = GetPosition().y;
            mainData.nodeType = nodeType;
            mainData.coroutineData = main.coroutine.CoroutineData;
            mainData.actionData = main.action.data;

            return mainData;
        }

        protected abstract void Initialize();
        protected abstract void SetupTitleContainer();

        private void UpdateSubContainers(NodeMainData mainData)
        {
            main.coroutine.DeserializeCoroutines(mainData.coroutineData);
            main.action.DeserializeActions(mainData.actionData);
        }
    }
}
#endif