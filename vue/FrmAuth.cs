using Mediatek86.controleur;
using Mediatek86.metier;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mediatek86.vue
{
    public partial class FrmAuth : Form
    {
        private readonly Controle controle;
        internal FrmAuth(Controle controle)
        {
            InitializeComponent();
            this.controle = controle;
        }

        /// <summary>
        /// Permet d'effectuer (ou non) la connexion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnexion_Click(object sender, EventArgs e)
        {
            if (txbidentifiant.Text.Equals("") || txbmdp.Text.Equals(""))
            {
                MessageBox.Show("Tous les champs doivent etre remplis", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Service service = controle.ControleAuthentification(txbidentifiant.Text, txbmdp.Text);
            if (service == null)
            {
                MessageBox.Show("Identifiants incorrects", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txbidentifiant.Text = "";
                txbmdp.Text = "";
                txbidentifiant.Focus();
                return;
            }
            if (service.ServiceInt == 3) //Service Culture
            {
                DialogResult result = MessageBox.Show("Vous n'avez pas les autorisations suffisantes pour utiliser l'application", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (result == DialogResult.OK)
                {
                    Environment.Exit(0);
                }
            }
            this.Hide();
            (new FrmMediatek(controle, service)).ShowDialog();
        }
    }
}
