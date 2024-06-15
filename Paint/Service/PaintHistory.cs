using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paint;

namespace Service
{
    [Serializable]
    public class PaintAction
    {
        [NonSerialized]
        public IShapeOwner owner;
        public virtual void Rollback() { }
    }

    [Serializable]
    class ShapeAction : PaintAction
    {
        string shapeId;
        Shape shape;
        public ShapeAction(string shapeId, Shape shape, IShapeOwner owner)
        {
            if (shape == null||owner==null) throw new ArgumentNullException();
            this.owner = owner;
            this.shapeId = shapeId;
            this.shape = shape;
        }

        public override void Rollback()
        {
            owner.ReplaceShape(shapeId, shape);
            if (shape.IsSelected) owner.SelectShape(shape);
        }
    }

    public interface IShapeOwner
    {
        void DeleteShape(string shapeId);
        void ReplaceShape(string shapeId, Shape newShape);
        void CreateShape(Shape shape);
        void SelectShape(Shape shape);
    }
    
    [Serializable]
    class DrawAction : PaintAction
    {
        Shape shape;
        public DrawAction(Shape shape, IShapeOwner owner)
        {
            if (shape == null || owner == null) throw new ArgumentNullException();
            this.shape = shape;
            this.owner = owner;
        }

        public override void Rollback()
        {
            owner.DeleteShape(shape.ID);
        }
    }
    
    [Serializable]
    class DeleteAction : PaintAction
    {
        Shape shape;
        public DeleteAction(Shape shape, IShapeOwner owner)
        {
            if (shape == null || owner == null) throw new ArgumentNullException();
            this.shape = shape;
            this.owner = owner;
        }

        public override void Rollback()
        {
            owner.CreateShape(shape);
            if (shape.IsSelected) owner.SelectShape(shape);
        }
    }

    [Serializable]
    public class History
    {
        int depth;
        Stack<PaintAction> actions;
        public bool HasActions()
        {
            return actions.Count != 0;
        }
        public History(int depth)
        {
            this.depth = depth;
            actions = new Stack<PaintAction>();
        }

        public void Add(PaintAction action)
        {
            if (actions.Count == depth) actions.Pop();

            actions.Push(action);
        }

        public PaintAction Last()
        {
            if (actions.Count == 0) return new PaintAction();

            return actions.Pop();
        }

        public List<PaintAction> GetActions()
        {
            return actions.Reverse().ToList();
        }

        public void SetActions(PaintAction[] actions)
        {
            this.actions.Clear();
            foreach(PaintAction action in actions)
            {
                this.actions.Push(action);
            }
        }
    }
}
