namespace OpenTkClient
{
    public static class MainApp
    {
        public static void Main()
        {
            using (var introForm = new IntroForm())
            {
                introForm.ShowDialog();
                Global.ServerName = introForm.ServerName;
                Global.IsLocalServer = introForm.IsLocalServer;
            }
            using (GameRenderer gameRenderer = new GameRenderer())
            {
                //Utilities.SetWindowTitle(example);
                if (Global.IsLocalServer)
                    Comms.Run(); // TODO
                else
                    Comms.Run();
                
                gameRenderer.Run(30.0);
            }
        }

    }
}

