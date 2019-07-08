namespace Ex03.Infrastracture.ObjectModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    public interface IBoundedComponent : IGameComponent, IDrawable
    {
        Vector2 Position { get; set; }

        Vector2 PositionOrigin { get; set; }

        float Width { get; set; }

        float Height { get; set; }
    }

    public class MultiBoundedComponent<TComponent> : CompositeDrawableComponent<TComponent>, IBoundedComponent
        where TComponent : IBoundedComponent
    {
        public Vector2 Position
        {
            get { return m_Position; }
            set
            {
                m_Position = value;
                OnPositionChanged();
            }
        }

        public Vector2 PositionOrigin
        {
            get { return m_PositionOrigin; }
            set { m_PositionOrigin = value; }
        }

        public float Width { get; set; }

        public float Height { get; set; }

        public float Gap
        {
            get { return m_Gap; }
            set { m_Gap = value; }
        }

        private Vector2 m_Position;
        private Vector2 m_PositionOrigin;
        private float m_Gap = 0;

        public MultiBoundedComponent(Game i_Game)
            : base(i_Game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            InitCompositeBounds();
        }

        public void Add(params TComponent[] i_Components)
        {
            foreach (TComponent component in i_Components)
            {
                base.Add(component);
            }
        }

        protected virtual void OnPositionChanged()
        {
            for (int i = 0; i < r_Components.Count; i++)
            {
                Vector2 position = new Vector2();
                position.X = this.Position.X + ((r_Components[i == 0 ? i : i - 1].Width + m_Gap) * i) - this.PositionOrigin.X;
                position.Y = this.Position.Y - this.PositionOrigin.Y;
                r_Components[i].Position = position;
            }
        }

        protected virtual void InitCompositeBounds()
        {
            for (int i = 0; i < r_Components.Count; i++)
            {
                r_Components[i].PositionOrigin = Vector2.Zero;
                this.Width += r_Components[i].Width;
                this.Height = MathHelper.Max(r_Components[i].Height, this.Height);
                if (i == r_Components.Count - 1)
                {
                    this.Width += i * m_Gap;
                }
            }

            this.PositionOrigin = new Vector2(this.Width / 2, this.Height / 2);
        }
    }
}
