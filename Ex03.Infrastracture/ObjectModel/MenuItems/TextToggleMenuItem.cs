namespace Ex03.Infrastracture.ObjectModel.MenuItems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Ex03.Infrastracture.ObjectModel.Sprites;

    public class TextToggleMenuItem : ToggleMenuItem<TextSprite>
    {
        public string ToggledMessege
        {
            get { return string.Format("{0} {1}", m_StaticText, m_Options[this.CurrentToggle]); }
        }

        private readonly List<string> m_Options;
        private string m_StaticText;

        public TextToggleMenuItem(string i_Name, TextSprite i_BoundedSprite, int i_DefaultToggleOption, bool i_Activatable, params string[] i_OptionsParams)
            : base(i_Name, i_BoundedSprite, i_OptionsParams.Length, i_DefaultToggleOption, 1, i_Activatable)
        {
            m_Options = new List<string>();
            m_StaticText = BoundedSprite.Text;

            foreach (string option in i_OptionsParams)
            {
                m_Options.Add(option);
            }
        }
        
        public override void Initialize()
        {
            base.Initialize();
            BoundedSprite.Text = this.ToggledMessege;
            BoundedSprite.InitBounds();
        }
        
        protected override void OnClicked()
        {
            this.BoundedSprite.Text = this.ToggledMessege;
            base.OnClicked();
        }
    }
}
