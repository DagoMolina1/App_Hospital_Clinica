using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Threading;
using App_Hospital_Clinica.Applicationn.UseCases.Pacientes.RegistrarPaciente;

namespace App_Hospital_Clinica {
    /// <summary>
    /// Ventana para registrar un nuevo paciente.
    /// Usa el caso de uso RegistrarPacienteHandler (Applicationn).
    /// </summary>
    public partial class RegistrarPacienteWindow : Window {
        private readonly RegistrarPacienteHandler _handler;

        // El handler llega por DI (lo registra DependencyInjection.AddClinicaServices)
        public RegistrarPacienteWindow(RegistrarPacienteHandler handler) {
            InitializeComponent();
            _handler = handler;
        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e) {
            try {
                // 1) Leer datos del formulario
                string cedula = TxtCedula.Text.Trim();
                string nombre = TxtNombre.Text.Trim();
                string genero = TxtGenero.Text.Trim();
                string direccion = TxtDireccion.Text.Trim();
                string telefono = TxtTelefono.Text.Trim();
                string correo = TxtCorreo.Text.Trim();
                DateTime? fechaNac = DpFechaNac.SelectedDate;

                // Validaciones mínimas del lado UI
                if (string.IsNullOrWhiteSpace(cedula) ||
                    string.IsNullOrWhiteSpace(nombre) ||
                    !fechaNac.HasValue ||
                    string.IsNullOrWhiteSpace(genero) ||
                    string.IsNullOrWhiteSpace(direccion) ||
                    string.IsNullOrWhiteSpace(telefono)) {
                    MessageBox.Show(
                        "Por favor diligencia todos los campos obligatorios (excepto correo).",
                        "Datos incompletos",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                // 2) Construir el comando para Applicationn
                // ⚠️ IMPORTANTE:
                // Ajusta esta parte según cómo esté definido EXACTAMENTE tu RegistrarPacienteCommand
                // (si es record posicional o clase con propiedades).
                var cmd = new RegistrarPacienteCommand {
                    Cedula = cedula,
                    NombreCompleto = nombre,
                    FechaNac = fechaNac.Value,
                    Genero = genero,
                    Direccion = direccion,
                    Telefono = telefono,
                    Correo = string.IsNullOrWhiteSpace(correo) ? null : correo
                };

                // 3) Ejecutar el caso de uso
                var result = await _handler.Handle(cmd, CancellationToken.None);

                if (!result.IsSuccess) {
                    MessageBox.Show(
                        result.Error ?? "No se pudo registrar el paciente.",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                MessageBox.Show(
                    "Paciente registrado correctamente.",
                    "Éxito",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                // Opcional: cerrar ventana después de guardar
                this.Close();
            } catch (Exception ex) {
                MessageBox.Show(
                    $"Ocurrió un error inesperado:\n{ex.Message}",
                    "Error inesperado",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}