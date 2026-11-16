namespace DisasterSafety.Game
{
    /// <summary>The words on one Tutorial Page. Art names the picture files.</summary>
    public readonly struct TutorialPageText
    {
        public readonly string Art;
        public readonly string Title;
        public readonly string Caption;
        public readonly string Tip;

        public TutorialPageText(string art, string title, string caption, string tip)
        {
            Art = art;
            Title = title;
            Caption = caption;
            Tip = tip;
        }
    }
}
