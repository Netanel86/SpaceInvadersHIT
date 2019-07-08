namespace Ex03.Infrastracture.ObjectModel.MenuItems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Ex03.Infrastracture.ObjectModel.Sprites;

    public class ValueToggleMenuItem : ToggleMenuItem<TextSprite>
    {
        public string ToggledMessege 
        { 
            get { return string.Format("{0} {1}", m_StaticText, m_CurrentToggle); } 
        }
        
        private string m_StaticText;

        public ValueToggleMenuItem(string i_Name, TextSprite i_BoundedSprite, int i_MaxValue, int i_DefaultMinValue, int i_Interval, bool i_Activatable)
            : base(i_Name, i_BoundedSprite, i_MaxValue + 1, i_DefaultMinValue, i_Interval, i_Activatable)
        {
            m_StaticText = i_BoundedSprite.Text;
        }

        public override void Initialize()
        {
            base.Initialize();
            BoundedSprite.Text = ToggledMessege;
        }
        
        protected override void OnClicked()
        {
            this.BoundedSprite.Text = ToggledMessege;
            base.OnClicked();
        }
    }
}
