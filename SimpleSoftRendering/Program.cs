using System.Windows.Forms;
using SSR.Implementations.SimpleTexture;

namespace SSR {
    internal static class Program {

        private static int Main(string[] args) {
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();
            Application.Run(new RenderForm());

            return 0;
        }

    }
}
