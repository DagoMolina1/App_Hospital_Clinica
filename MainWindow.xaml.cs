using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using App_Hospital_Clinica.Infrastructure_SqlServer.EfDb;

namespace App_Hospital_Clinica
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /*public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }*/
    public partial class MainWindow : Window {
        private readonly ClinicaDbContext _db;

        public MainWindow(ClinicaDbContext db) {
            InitializeComponent();
            _db = db;

            // Aquí ya puedes usar _db.Pacientes, _db.Ordenes, etc.
        }
    }
}