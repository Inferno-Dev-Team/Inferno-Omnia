using System.Windows.Controls;
using System.Windows.Input;
using Steamworks.Data;

namespace Inferno_Mod_Manager.Controller
{
    /// <summary>
    /// Interaction logic for AchievementPanel.xaml
    /// </summary>
    public partial class AchievementPanel : UserControl
    {
        private int the = 0;
        public AchievementPanel()
        {
            InitializeComponent();
        }

        private string _name = "???";
        public string achName { get => _name; set => _name = value; }
        public Achievement? __As_0x32459; //if i remove code breaks

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            the++;
            if (the > ((1 << 7) - 28))
                __As_0x32459?.Trigger();
        }
    }
}
