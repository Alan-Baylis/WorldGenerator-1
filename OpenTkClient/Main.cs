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
                IServer server;
                if (Global.IsLocalServer)
                    server = new ServerLocal ();
                else
                    server = new ServerRemote ();

                Server.Run (server);
                
                gameRenderer.Run(30.0);
            }
        }

    }
}

