﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SportGest
{
    public partial class NuevoEquipo : Form
    {
        public bool editar = false, error = false;
        public string id;
        private int num;
        string sCnn = Properties.Settings.Default.SportGestConnection;
        public NuevoEquipo()
        {
            InitializeComponent();
        }

        private void lbObjetivos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void NuevoEquipo_Load(object sender, EventArgs e)
        {
            if (editar)
            {
                btnAñadirEquipo.Text = "Acpetar cambios";
                this.Text = "Equipo";

                using (SQLiteConnection connection = new SQLiteConnection(sCnn))
                {
                    SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM [Equipos]", connection);
                    cmd.CommandType = CommandType.Text;
                    try
                    {
                        connection.Open();
                        SQLiteDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            if (dr["Id"].ToString().Equals(id))
                            {
                                tbNumero.Text = dr["num_jugadores"].ToString();
                                tbNombre.Text = dr["nombre"].ToString();
                                tbLiga.Text = dr["liga"].ToString();
                                tbObservaciones.Text = dr["observaciones"].ToString();
                                cbCategoria.SelectedItem = dr["categoria"].ToString();
                            }
                        }
                    }
                    catch (SQLiteException exc)
                    {
                        MessageBox.Show(exc.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            else
            {
                btnAñadirEquipo.Text = "Añadir";
                this.Text = "Nuevo equipo";
            }

        }

        private void btnAñadirEquipo_Click(object sender, EventArgs e)
        {
            try
            {
                num = int.Parse(tbNumero.Text);
            }
            catch (FormatException)
            {
                num = 0;
            }
            error = false;
            for (int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i] is TextBox && this.Controls[i].Text.Equals("") && !this.Controls[i].Name.Equals("tbObservaciones") && !this.Controls[i].Name.Equals("tbNumero") && !this.Controls[i].Name.Equals("tbLiga"))
                {
                    error = true;
                }
            }
            if (!error)
            {
                if (!editar)
                {
                    using (SQLiteConnection connection = new SQLiteConnection(sCnn))
                    {
                        //equiposAdapter.Insert(tbNombre.Text, cbCategoria.SelectedItem.ToString(), tbLiga.Text, int.Parse(tbNumero.Text), tbObservaciones.Text);
                        SQLiteCommand cmd = new SQLiteCommand("INSERT into [Equipos] (nombre,categoria,liga,num_jugadores,observaciones) VALUES(@nombre,@categoria,@liga,@numero_jugadores,@observaciones)", connection);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@categoria", cbCategoria.Text);
                        cmd.Parameters.AddWithValue("@nombre", tbNombre.Text);
                        cmd.Parameters.AddWithValue("@liga", tbLiga.Text);
                        cmd.Parameters.AddWithValue("@numero_jugadores", num);
                        cmd.Parameters.AddWithValue("@observaciones", tbObservaciones.Text);
                        try
                        {
                            connection.Open();
                            int fa = cmd.ExecuteNonQuery();
                            if (fa >= 1)
                            {
                                MessageBox.Show("Añadido", "Añadir equipo", MessageBoxButtons.OK);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
                else
                {
                    using (SQLiteConnection connection = new SQLiteConnection(sCnn))
                    {
                        SQLiteCommand cmd = new SQLiteCommand("UPDATE [Equipos] SET " +
                            "nombre=@nombre," +
                            "categoria=@categoria," +
                            "liga=@liga," +
                            "num_jugadores=@numero_jugadores," +
                            "observaciones=@observaciones  WHERE id=@id", connection);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@categoria", cbCategoria.Text);
                        cmd.Parameters.AddWithValue("@nombre", tbNombre.Text);
                        cmd.Parameters.AddWithValue("@liga", tbLiga.Text);
                        cmd.Parameters.AddWithValue("@numero_jugadores", num);
                        cmd.Parameters.AddWithValue("@observaciones", tbObservaciones.Text);
                        cmd.Parameters.AddWithValue("@id", id);
                        try
                        {
                            connection.Open();
                            int fa = cmd.ExecuteNonQuery();
                            if (fa == 1)
                            {
                                MessageBox.Show("Operación correcta", "Modificar", MessageBoxButtons.OK);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Algún campo vacío", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
