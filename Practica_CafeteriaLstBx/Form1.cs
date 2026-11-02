using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Practica_CafeteriaLstBx
{
    public partial class Form1 : Form
    {
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x2, 0);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // validar si el usuario quiere salir de la aplicacion
            DialogResult result = MessageBox.Show("¿Está seguro de que desea salir de la aplicación?", "Confirmar salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // minimizar la ventana
            this.WindowState = FormWindowState.Minimized;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbPlatillos.SelectedItem = 0;
        }

        private void cmbPlatillos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ruta = "";
            switch (cmbPlatillos.SelectedItem.ToString())
            {
                case "Pollo a la mantequilla  - C$ 116.46":
                    lsMenu.SelectedItem = "Pollo a la mantequilla  - C$ 116.46";
                    ruta = @"Resources\polloAlaMantequilla.jpg";
                    break;
                case "Arroz Biryani con pollo - C$ 97.05":
                    lsMenu.SelectedItem = "Arroz Biryani con pollo - C$ 97.05";
                    ruta = @"Resources\arrozbiryani.jpg";
                    break;
                case "Brochetas Kebab de carne - C$ 58.20":
                    lsMenu.SelectedItem = "Brochetas Kebab de carne - C$ 58.20";
                    ruta = @"Resources\brochetasDeKebab.jpg";
                    break;
                case "Pan Naan con ajo - C$ 19.41":
                    lsMenu.SelectedItem = "Pan Naan con ajo - C$ 19.41";
                    ruta = @"Resources\panNaan.jpg";
                    break;
                case "Empanadas indias (Samosas) - C$ 15.53":
                    lsMenu.SelectedItem = "Empanadas indias (Samosas) - C$ 15.53";
                    ruta = @"Resources\Samosas.jpg";
                    break;
                case "Batido de mango-yogur (Mango Lassi) - C$ 31.04":
                    lsMenu.SelectedItem = "Batido de mango-yogur (Mango Lassi) - C$ 31.04";
                    ruta = @"Resources\bebidaMango.jpg";
                    break;
                case "Limonada india (Nimbu Pani) - C$ 15.52":
                    lsMenu.SelectedItem = "Limonada india (Nimbu Pani) - C$ 15.52";
                    ruta = @"Resources\LimonadaIndia.jpg";
                    break;
            }
            /*
             Pollo a la mantequilla  - C$ 116.46
Arroz Biryani con pollo - C$ 97.05
Brochetas Kebab de carne - C$ 58.20
Pan Naan con ajo - C$ 19.41
Empanadas indias (Samosas) - C$ 15.53
Batido de mango-yogur (Mango Lassi) - C$ 31.04
Limonada india (Nimbu Pani) - C$ 15.52
             */


            picPlatillo.Image = Image.FromFile(ruta);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            // vaciar listbox 
            lsPedido.Items.Clear();
        }

        private void btnDevolver_Click(object sender, EventArgs e)
        {
            // eliminar item seleccionado del listbox si hay seleccion, sino eliminar el ultimo elemento del listbox
            if (lsPedido.SelectedItem != null)
            {
                lsPedido.Items.Remove(lsPedido.SelectedItem);
            }
            else if (lsPedido.Items.Count > 0)
            {
                lsPedido.Items.RemoveAt(lsPedido.Items.Count - 1);
            }
            else
            {
                MessageBox.Show("No hay platillos en el pedido.", "Pedido vacío", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // hacer que se agregue el item seleccionado del listbox o combobox al listbox de pedido, si no hay seleccion mostrar un mensaje de error
            if (lsMenu.SelectedItem != null)
            {
                lsPedido.Items.Add(lsMenu.SelectedItem.ToString());
            }
            else if (cmbPlatillos.SelectedItem != null)
            {
                lsPedido.Items.Add(cmbPlatillos.SelectedItem.ToString());
            }
            else
            {
                MessageBox.Show("No hay platillos seleccionados.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            // calcular el total del pedido y mostrarlo en un MessageBox
            float total = 0;
            if (lsPedido.Items.Count > 0)
            {
                foreach (var item in lsPedido.Items)
                {
                    string platillo = item.ToString();
                    string[] partes = platillo.Split('-');
                    if (partes.Length == 2)
                    {
                        string precioStr = partes[1].Trim().Replace("C$", "").Trim();
                        if (float.TryParse(precioStr, out float precio))
                        {
                            total += precio;
                        }
                    }
                }
                string detallePedido = "Detalle del pedido:\n\n";
                foreach (var item in lsPedido.Items)
                {
                    detallePedido += $"{item}\n";
                }
                MessageBox.Show($"{detallePedido}\nEl total de su pedido es: C$ {total:F2}", "Total del pedido", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No hay platillos en el pedido.", "Pedido vacío", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            lsPedido.Items.Clear();

        }

        private void lsMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            // hacer que al seleccionar un item del listbox se busque en el combobox y se seleccione el mismo item, si no existe mostrar un mensaje de error
            if (lsMenu.SelectedItem != null)
            {
                string selectedItem = lsMenu.SelectedItem.ToString();
                bool found = false;
                foreach (var item in cmbPlatillos.Items)
                {
                    if (item.ToString() == selectedItem)
                    {
                        cmbPlatillos.SelectedItem = item;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    MessageBox.Show("Ha seleccionado un espacio en blanco, presione un platillo del menú existente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
