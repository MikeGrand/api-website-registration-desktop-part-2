using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Registro
{
    public partial class Form1 : Form
    {
        string BASE_URL = "http://localhost/api/index.php";
        string MY_KEY = "1234";

        public Form1()
        {
            InitializeComponent();
            PersonalizarDiseño();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _ = btnConsultaAsync();
        }

        private void PersonalizarDiseño()
        {
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ForeColor = Color.White;
            this.Text = "Agenda Digital";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            EstilarBoton(button1, Color.FromArgb(40, 167, 69));
            button1.Text = "Guardar";

            EstilarBoton(button2, Color.FromArgb(0, 123, 255));
            button2.Text = "Editar";

            EstilarBoton(button3, Color.FromArgb(220, 53, 69));
            button3.Text = "Eliminar";

            dataGridView1.BackgroundColor = Color.FromArgb(45, 45, 48);
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.GridColor = Color.FromArgb(60, 60, 60);
            dataGridView1.EnableHeadersVisualStyles = false;

            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(64, 64, 64);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
            dataGridView1.DefaultCellStyle.ForeColor = Color.White;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;

            foreach (Control c in this.Controls)
            {
                if (c is TextBox)
                {
                    c.BackColor = Color.FromArgb(60, 60, 60);
                    c.ForeColor = Color.White;
                    ((TextBox)c).BorderStyle = BorderStyle.FixedSingle;
                }
            }
        }

        private void EstilarBoton(Button btn, Color colorFondo)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = colorFondo;
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        }

        private bool ValidarIdEnTabla(string idBuscado)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == idBuscado)
                {
                    return true;
                }
            }
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _ = btnAcrear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _ = btnModificar();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _ = btnEliminar();
        }

        public async Task btnAcrear()
        {
            String nom = textBox2.Text.Trim();
            String app = textBox3.Text.Trim();
            String tel = textBox4.Text.Trim();

            if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(app) || string.IsNullOrEmpty(tel))
            {
                MessageBox.Show("Por favor, llena todos los campos.", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            String url = $"{BASE_URL}?tipo=2&nom={nom}&app={app}&tel={tel}&llave={MY_KEY}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    await btnConsultaAsync();
                    MessageBox.Show("Guardado con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCajas();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al agregar: " + ex.Message);
                }
            }
        }

        public async Task btnModificar()
        {
            String id = textBox1.Text.Trim();
            String nom = textBox2.Text.Trim();
            String app = textBox3.Text.Trim();
            String tel = textBox4.Text.Trim();

            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Selecciona un registro de la tabla.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidarIdEnTabla(id))
            {
                MessageBox.Show("El ID ingresado no existe en la base de datos actual.", "Error de ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(app) || string.IsNullOrEmpty(tel))
            {
                MessageBox.Show("No puedes dejar campos vacíos.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            String url = $"{BASE_URL}?tipo=3&id={id}&nom={nom}&app={app}&tel={tel}&llave={MY_KEY}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    await btnConsultaAsync();
                    MessageBox.Show("Modificado con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCajas();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al modificar: " + ex.Message);
                }
            }
        }

        public async Task btnEliminar()
        {
            String id = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Selecciona un registro para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidarIdEnTabla(id))
            {
                MessageBox.Show("El ID que intentas eliminar no existe.", "Error de ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult dialogResult = MessageBox.Show("¿Seguro que quieres eliminar este registro?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.No) return;

            String url = $"{BASE_URL}?tipo=4&id={id}&llave={MY_KEY}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    await btnConsultaAsync();
                    MessageBox.Show("Eliminado con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCajas();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message);
                }
            }
        }

        public async Task btnConsultaAsync()
        {
            string url = $"{BASE_URL}?tipo=1&llave={MY_KEY}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    String result = await response.Content.ReadAsStringAsync();
                    Root datos = JsonConvert.DeserializeObject<Root>(result);

                    this.dataGridView1.Rows.Clear();

                    if (datos != null && datos.lista != null)
                    {
                        for (int i = 0; i < datos.lista.Count; i++)
                        {
                            this.dataGridView1.Rows.Add(
                                datos.lista[i].id,
                                datos.lista[i].nom,
                                datos.lista[i].app,
                                datos.lista[i].tel
                            );
                        }
                    }
                    this.dataGridView1.ClearSelection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error de conexión: " + ex.Message);
                }
            }
        }

        private void LimpiarCajas()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            LlenarDesdeTabla();
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            LlenarDesdeTabla();
        }

        private void LlenarDesdeTabla()
        {
            if (dataGridView1.CurrentRow != null && dataGridView1.CurrentRow.Index > -1)
            {
                int x = dataGridView1.CurrentCell.RowIndex;

                textBox1.Text = dataGridView1.Rows[x].Cells[0].Value?.ToString();
                textBox2.Text = dataGridView1.Rows[x].Cells[1].Value?.ToString();
                textBox3.Text = dataGridView1.Rows[x].Cells[2].Value?.ToString();
                textBox4.Text = dataGridView1.Rows[x].Cells[3].Value?.ToString();
            }
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void textBox4_TextChanged(object sender, EventArgs e) { }
    }

    public class Root
    {
        public List<Dato> lista { get; set; }
    }

    public class Dato
    {
        public string id { get; set; }
        public string nom { get; set; }
        public string app { get; set; }
        public string tel { get; set; }
        public string clave { get; set; }
    }
}