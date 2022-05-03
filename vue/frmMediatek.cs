using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Mediatek86.metier;
using Mediatek86.controleur;
using System.Drawing;
using System.Linq;

namespace Mediatek86.vue
{
    public partial class FrmMediatek : Form
    {

        #region Variables globales

        private readonly Controle controle;
        const string ETATNEUF = "00001";

        private readonly BindingSource bdgLivresListe = new BindingSource();
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private readonly BindingSource bdgCommandesListeLivres = new BindingSource();
        private readonly BindingSource bdgCommandesListeDvd = new BindingSource();
        private readonly BindingSource bdgSuivisListe = new BindingSource();
        private readonly BindingSource bdgAbonnementsListeRevues = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();
        private List<CommandeDocumentLivre> lesCommandesLivre = new List<CommandeDocumentLivre>();
        private List<CommandeDocumentDvd> lesCommandesDvd = new List<CommandeDocumentDvd>();
        private List<AbonnementRevue> lesAbonnementsRevues = new List<AbonnementRevue>();
        private List<Dvd> lesDvd = new List<Dvd>();
        private List<Revue> lesRevues = new List<Revue>();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        private Service service;

        private bool ajoutCommandeLivre = false;
        private bool modifCommandeLivre = false;
        private bool ajoutCommandeDVD = false;
        private bool modifCommandeDVD = false;
        #endregion


        public FrmMediatek(Controle controle, Service service)
        {
            InitializeComponent();
            this.controle = controle;
            this.service = service;
        }


        #region modules communs

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories"></param>
        /// <param name="bdg"></param>
        /// <param name="cbx"></param>
        private void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Quand on quitte l'onglet permet de reinitialiser les booleans et de redisable les GroupBox 
        /// </summary>
        private void BloquerAjoutModif()
        {
            BloquerAjtModifCommandeDVD();
            BloquerAjtModifCommandeLivres();
            grpAjtAbonnement.Enabled = false;
        }

        #endregion


        #region Revues
        //-----------------------------------------------------------
        // ONGLET "Revues"
        //------------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controle.GetAllRevues();
            RemplirComboCategorie(controle.GetAllGenres(), bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(controle.GetAllPublics(), bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(controle.GetAllRayons(), bdgRayons, cbxRevuesRayons);
            RemplirRevuesListeComplete();
            BloquerAjoutModif();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns["empruntable"].Visible = false;
            dgvRevuesListe.Columns["idRayon"].Visible = false;
            dgvRevuesListe.Columns["idGenre"].Visible = false;
            dgvRevuesListe.Columns["idPublic"].Visible = false;
            dgvRevuesListe.Columns["image"].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>();
                    revues.Add(revue);
                    RemplirRevuesListe(revues);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue"></param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            chkRevuesEmpruntable.Checked = revue.Empruntable;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            chkRevuesEmpruntable.Checked = false;
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                }
                catch
                {
                    VideRevuesZones();
                }
            }
            else
            {
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }

        #endregion


        #region Livres

        //-----------------------------------------------------------
        // ONGLET "LIVRES"
        //-----------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            if (service.ServiceInt == 2) //Service pret
            {
                dgvLivresListeGestion.TabPages.RemoveByKey("tabReceptionRevue");
                dgvLivresListeGestion.TabPages.RemoveByKey("tabGestionLivres");
                dgvLivresListeGestion.TabPages.RemoveByKey("tabGestionCmdDvd");
                dgvLivresListeGestion.TabPages.RemoveByKey("tabGestionCmdRevues");
            }
            lesLivres = controle.GetAllLivres();
            RemplirComboCategorie(controle.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(controle.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(controle.GetAllRayons(), bdgRayons, cbxLivresRayons);
            RemplirLivresListeComplete();
            BloquerAjoutModif();
            if (service.ServiceInt == 1)
            {
                string procedure = controle.GetAbonnementsSub30Days();
                if (procedure != "")
                {
                    MessageBox.Show(procedure, "Abonnements finissants dans moins de 30 jours");
                }
            }
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>();
                    livres.Add(livre);
                    RemplirLivresListe(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre"></param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            txbLivresGenre.Text = livre.Genre;
            txbLivresPublic.Text = livre.Public;
            txbLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            txbLivresGenre.Text = "";
            txbLivresPublic.Text = "";
            txbLivresRayon.Text = "";
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }

        #endregion


        #region Dvd
        //-----------------------------------------------------------
        // ONGLET "DVD"
        //-----------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet Dvds : 
        /// appel des méthodes pour remplir le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controle.GetAllDvd();
            RemplirComboCategorie(controle.GetAllGenres(), bdgGenres, cbxDvdGenres);
            RemplirComboCategorie(controle.GetAllPublics(), bdgPublics, cbxDvdPublics);
            RemplirComboCategorie(controle.GetAllRayons(), bdgRayons, cbxDvdRayons);
            RemplirDvdListeComplete();
            BloquerAjoutModif();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void RemplirDvdListe(List<Dvd> Dvds)
        {
            bdgDvdListe.DataSource = Dvds;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns["idRayon"].Visible = false;
            dgvDvdListe.Columns["idGenre"].Visible = false;
            dgvDvdListe.Columns["idPublic"].Visible = false;
            dgvDvdListe.Columns["image"].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>();
                    Dvd.Add(dvd);
                    RemplirDvdListe(Dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des Dvd dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd"></param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            txbDvdDuree.Text = dvd.Duree.ToString();
            txbDvdNumero.Text = dvd.Id;
            txbDvdGenre.Text = dvd.Genre;
            txbDvdPublic.Text = dvd.Public;
            txbDvdRayon.Text = dvd.Rayon;
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            txbDvdDuree.Text = "";
            txbDvdNumero.Text = "";
            txbDvdGenre.Text = "";
            txbDvdPublic.Text = "";
            txbDvdRayon.Text = "";
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    AfficheDvdInfos(dvd);
                }
                catch
                {
                    VideDvdZones();
                }
            }
            else
            {
                VideDvdInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des Dvd
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }

        #endregion


        #region Réception Exemplaire de presse
        //-----------------------------------------------------------
        // ONGLET "RECEPTION DE REVUES"
        //-----------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet : blocage en saisie des champs de saisie des infos de l'exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controle.GetAllRevues();
            accesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            bdgExemplairesListe.DataSource = exemplaires;
            dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
            dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
            dgvReceptionExemplairesListe.Columns["idDocument"].Visible = false;
            dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
            dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    VideReceptionRevueInfos();
                }
            }
            else
            {
                VideReceptionRevueInfos();
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            accesReceptionExemplaireGroupBox(false);
            VideReceptionRevueInfos();
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue"></param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            chkReceptionRevueEmpruntable.Checked = revue.Empruntable;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            afficheReceptionExemplairesRevue();
            // accès à la zone d'ajout d'un exemplaire
            accesReceptionExemplaireGroupBox(true);
        }

        private void afficheReceptionExemplairesRevue()
        {
            string idDocuement = txbReceptionRevueNumero.Text;
            lesExemplaires = controle.GetExemplairesRevue(idDocuement);
            RemplirReceptionExemplairesListe(lesExemplaires);
        }

        /// <summary>
        /// Vide les zones d'affchage des informations de la revue
        /// </summary>
        private void VideReceptionRevueInfos()
        {
            txbReceptionRevuePeriodicite.Text = "";
            chkReceptionRevueEmpruntable.Checked = false;
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            lesExemplaires = new List<Exemplaire>();
            RemplirReceptionExemplairesListe(lesExemplaires);
            accesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de l'exemplaire
        /// </summary>
        private void VideReceptionExemplaireInfos()
        {
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces"></param>
        private void accesReceptionExemplaireGroupBox(bool acces)
        {
            VideReceptionExemplaireInfos();
            grpReceptionExemplaire.Enabled = acces;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        /// <summary>
        /// Enregistrement du nouvel exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                    if (controle.CreerExemplaire(exemplaire))
                    {
                        VideReceptionExemplaireInfos();
                        afficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("numéro de publication déjà existant", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Photo":
                    sortedList = lesExemplaires.OrderBy(o => o.Photo).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// Sélection d'une ligne complète et affichage de l'image sz l'exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }

        #endregion

        #region Gestion de livres
        /// <summary>
        /// Quand on arrive sur l'onglet
        /// 
        /// Initialise la DataGridView
        /// Remplit les ComboBox correspondantes aux livres dans Ajouter et aux differentes etapes de suivi dans Modifier
        /// Disable les GroupBox d'ajout et de modification des commandes et met les booleans a false
        /// Vide le contenu de la zone de modification au cas ou l'utilisateur a quitter l'onglet en pleine modification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabGestionLivres_Enter(object sender, EventArgs e)
        {
            lesCommandesLivre = controle.GetCommandesLivres();
            lesAbonnementsRevues = controle.GetAbonnementsRevues();
            bdgAbonnementsListeRevues.DataSource = lesAbonnementsRevues;
            lesCommandesDvd = controle.GetCommandesDvd();
            bdgCommandesListeDvd.DataSource = lesCommandesDvd;
            InitDataGridViewLivreCommande();
            RemplirComboBoxLivresCommande();
            BloquerAjoutModif();
            ViderEditCommandeLivre();
            ViderAjouterCommandeLivre();
        }
        /// <summary>
        /// Remplit la DataGridView et masque des colonnes
        /// </summary>
        private void InitDataGridViewLivreCommande()
        {
            List<CommandeDocumentLivre> livres = controle.GetCommandesLivres();
            bdgCommandesListeLivres.DataSource = livres;
            dgvLivresListeCommande.DataSource = bdgCommandesListeLivres;
            dgvLivresListeCommande.Columns["Id"].Visible = false;
            dgvLivresListeCommande.Columns["IdLivDVD"].Visible = false;
            dgvLivresListeCommande.Columns["idSuivi"].Visible = false;
            dgvLivresListeCommande.Columns["ISBN"].Visible = false;
            dgvLivresListeCommande.Columns["Titre"].Visible = false;
            dgvLivresListeCommande.Columns["Auteur"].Visible = false;
            dgvLivresListeCommande.Columns["Collection"].Visible = false;
            dgvLivresListeCommande.Columns["Genre"].Visible = false;
            dgvLivresListeCommande.Columns["Public"].Visible = false;
            dgvLivresListeCommande.Columns["Rayon"].Visible = false;
            dgvLivresListeCommande.Columns["image"].Visible = false;

            dgvLivresListeCommande.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Affiche les resultats d'une recherche par numero de document (Livre)
        /// </summary>
        /// <param name="livres"></param>
        private void InitDataGridViewRechercheLivreCommande(List<CommandeDocumentLivre> livres)
        {
            bdgCommandesListeLivres.DataSource = livres;
            dgvLivresListeCommande.DataSource = bdgCommandesListeLivres;
            dgvLivresListeCommande.Columns["Id"].Visible = false;
            dgvLivresListeCommande.Columns["IdLivDVD"].Visible = false;
            dgvLivresListeCommande.Columns["idSuivi"].Visible = false;
            dgvLivresListeCommande.Columns["ISBN"].Visible = false;
            dgvLivresListeCommande.Columns["Titre"].Visible = false;
            dgvLivresListeCommande.Columns["Auteur"].Visible = false;
            dgvLivresListeCommande.Columns["Collection"].Visible = false;
            dgvLivresListeCommande.Columns["Genre"].Visible = false;
            dgvLivresListeCommande.Columns["Public"].Visible = false;
            dgvLivresListeCommande.Columns["Rayon"].Visible = false;
            dgvLivresListeCommande.Columns["image"].Visible = false;

            dgvLivresListeCommande.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        /// <summary>
        /// Quand la selection de ligne est changée (marche aussi quand on arrive sur la page)
        /// Les informations du livre sont remplies dans les champs correspondants
        /// 
        /// Si aucune ligne n'est selectionnée, on vide tout les champs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvLivresListeCommande_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListeCommande.CurrentCell != null)
            {
                try
                {
                    CommandeDocumentLivre commandeSelec = (CommandeDocumentLivre)bdgCommandesListeLivres.List[bdgCommandesListeLivres.Position];
                    AfficheLivreCommandeInfos(commandeSelec);
                    if (modifCommandeLivre)
                    {
                        RemplirModifCommandeLivre(commandeSelec);
                    }
                }
                catch
                {
                    txbCLivresRecherche.Text = "";
                }
            }
            else
            {
                VideLivresCommandeInfos();
            }
        }

        /// <summary>
        /// Affiche les infos du livre envoyé dans les champs
        /// </summary>
        /// <param name="livre"></param>
        private void AfficheLivreCommandeInfos(CommandeDocumentLivre livre)
        {
            string image = livre.Image;
            txtCLivresNumDoc.Text = livre.IdLivDVD;
            txtCLivresISBN.Text = livre.ISBN;
            txtCLivresTitre.Text = livre.Titre;
            txtCLivresAuteur.Text = livre.Auteur;
            txtCLivresCollection.Text = livre.Collection;
            txtCLivresPublic.Text = livre.Public;
            txtCLivresRayon.Text = livre.Rayon;
            txbLivresImage.Text = image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Vide tout les champs correspondant aux livres
        /// </summary>
        private void VideLivresCommandeInfos()
        {
            txtCLivresNumDoc.Text = "";
            txtCLivresISBN.Text = "";
            txtCLivresTitre.Text = "";
            txtCLivresAuteur.Text = "";
            txtCLivresCollection.Text = "";
            txtCLivresPublic.Text = "";
            txtCLivresRayon.Text = "";
            txbLivresImage.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Permet de trouver toutes les commandes pour un id de livre correspondant
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RechercherNumeroCommandeLivre_Click(object sender, EventArgs e)
        {
            if (!txbCLivresRecherche.Text.Equals(""))
            {
                List<CommandeDocumentLivre> livres = lesCommandesLivre.FindAll(x => x.IdLivDVD.Equals(txbCLivresRecherche.Text));
                txbCLivresRecherche.Text = "";
                if (livres.Any())
                {
                    InitDataGridViewRechercheLivreCommande(livres);
                }
                else
                {
                    MessageBox.Show("Numéro introuvable");
                    InitDataGridViewLivreCommande();
                }
            }
            else
            {
                InitDataGridViewLivreCommande();
            }
        }

        /// <summary>
        /// Remplit les ComboBox correspondantes avec les tout les Livres et Etats possibles
        /// </summary>
        private void RemplirComboBoxLivresCommande()
        {
            List<Livre> livres = controle.GetAllLivres();
            bdgLivresListe.DataSource = livres;
            cbxCLivreLivreCommande.DataSource = bdgLivresListe;
            if (cbxCLivreLivreCommande.Items.Count > 0)
            {
                cbxCLivreLivreCommande.SelectedIndex = 0;
            }

            List<Suivi> document = controle.GetAllSuivis();
            bdgSuivisListe.DataSource = document;
            cmbCLivreEtapeSuivi.DataSource = bdgSuivisListe;
            if (cmbCLivreEtapeSuivi.Items.Count > 0)
            {
                cmbCLivreEtapeSuivi.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Disable les groupes et met les booleans a false
        /// </summary>
        private void BloquerAjtModifCommandeLivres()
        {
            grpAjoutCLivres.Enabled = false;
            grpCLivresSuivi.Enabled = false;
            ajoutCommandeLivre = false;
            modifCommandeLivre = false;
        }

        /// <summary>
        /// Active la GroupBox correspondante et met le boolean a true
        /// 
        /// Si une commande est en cours de modif, elle desactive et vide la GroupBox de modification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAjoutCommandeLivres_Click(object sender, EventArgs e)
        {
            grpAjoutCLivres.Enabled = true;
            ajoutCommandeLivre = true;

            if (modifCommandeLivre)
            {
                grpCLivresSuivi.Enabled = false;
                modifCommandeLivre = false;
                ViderEditCommandeLivre();
            }
        }

        /// <summary>
        /// Si aucune ligne n'est séléctionner, l'utilisateur en est informé
        /// 
        /// Active la GroupBox correspondante et met le boolean sur true
        /// Si un ajout étais en cours, la GroupBox est vidée et disabled
        /// 
        /// Remplit la zone de modification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifierCommandeLivres_Click(object sender, EventArgs e)
        {

            if (dgvLivresListeCommande.CurrentCell != null)
            {
                CommandeDocumentLivre laCommande = (CommandeDocumentLivre)bdgCommandesListeLivres.List[bdgCommandesListeLivres.Position];

                if (laCommande.Etat == "Réglée.")
                {
                    MessageBox.Show("Cette commande est déja réglée, impossible de retourner en arrière", "Erreur");
                    return;
                }

                grpCLivresSuivi.Enabled = true;
                modifCommandeLivre = true;

                if (ajoutCommandeLivre)
                {
                    grpAjoutCommandeDVD.Enabled = false;
                    ajoutCommandeDVD = false;
                    ViderAjouterCommandeLivre();
                }

                RemplirModifCommandeLivre(laCommande);
            }
            else
            {
                MessageBox.Show("Veuillez selectionner une commande", "Erreur");
            }
        }

        /// <summary>
        /// Si la commande est dans un état trop avancé (Livré ou Réglé) on ne peut pas selectionner changer la valeur a "En cours." ou "Rélancée."
        /// L'état d'une commande ne peut etre changer sur livrée seulement si la commande a été réglée avant
        /// 
        /// Update les valeurs dans la BDD et actualise la DataGridView
        /// Disable le groupe et vide tout les champs
        /// Met le boolean a false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifCompleteLivres_Click(object sender, EventArgs e)
        {
            Suivi suivi = (Suivi)bdgSuivisListe.List[bdgSuivisListe.Position];
            string suiviId = suivi.Id;
            string suiviLibelle = suivi.Libelle;
            CommandeDocumentLivre laCommande = (CommandeDocumentLivre)bdgCommandesListeLivres.List[bdgCommandesListeLivres.Position];
            if ((laCommande.Etat == "Réglée." || laCommande.Etat == "Livrée.") && ((suiviLibelle == "En cours.") || (suiviLibelle == "Relancée.")))
            {
                MessageBox.Show("La commande est dans un stade trop avancé pour revenir a cet état", "Erreur");
                RemplirModifCommandeLivre(laCommande);
                return;
            }
            if (suiviLibelle == "Réglée." && laCommande.Etat != "Livrée.")
            {
                MessageBox.Show("La commande ne peut etre réglée sans être livrée avant.", "Erreur");
                RemplirModifCommandeLivre(laCommande);
                return;
            }

            bool resultat = controle.ModifierCommandeLivreDVD(laCommande.Id, suiviId);
            InitDataGridViewLivreCommande();
            lesCommandesLivre = controle.GetCommandesLivres();
            bdgCommandesListeLivres.DataSource = controle.GetCommandesLivres();
            ViderEditCommandeLivre();
            modifCommandeLivre = false;
            grpCLivresSuivi.Enabled = false;

            if (!resultat)
            {
                MessageBox.Show("Une erreur est survenue", "Erreur");
            }
        }

        /// <summary>
        /// Si on annule l'edition d'une commande, les infos dans les groupe sont vidées, le groupe est disable et le boolean est set à false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnulerEditCommandLivre_Click(object sender, EventArgs e)
        {
            ViderEditCommandeLivre();
            modifCommandeLivre = false;
            grpCLivresSuivi.Enabled = false;
        }

        /// <summary>
        /// Vide le contenu de la GroupBox d'édition
        /// </summary>
        private void ViderEditCommandeLivre()
        {
            txbCLivreIdSuivi.Text = "";
            cmbCLivreEtapeSuivi.SelectedIndex = 0;
        }

        /// <summary>
        /// Supprime la commande séléctionée dans la BDD après acceptation par l'utilisateur et actualise la DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprimerCommandeLivres_Click(object sender, EventArgs e)
        {
            if (dgvLivresListeCommande.CurrentCell != null)
            {
                CommandeDocumentLivre laCommande = (CommandeDocumentLivre)bdgCommandesListeLivres.List[bdgCommandesListeLivres.Position];
                if (laCommande.Etat == "Livrée." || laCommande.Etat == "Réglée.")
                {
                    MessageBox.Show("La commande est dans un stade trop avancé pour être supprimée");
                    return;
                }
                DialogResult dialogResult = MessageBox.Show($"Êtes vous sur(e) de vouloir supprimer la commande ayant pour id : {laCommande.Id}", "Confirmer", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    bool resultat = controle.SupprimerCommandeLivreDVD(laCommande.Id);
                    InitDataGridViewLivreCommande();
                    if (!resultat)
                    {
                        MessageBox.Show("Une erreur est survenue", "Erreur");
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    InitDataGridViewLivreCommande();
                }
            }
            else
            {
                MessageBox.Show("Veuillez selectionner une commande", "Erreur");
            }
        }

        /// <summary>
        /// Remplit les champs de la GroupBox d'édition avec les valeurs du livre
        /// </summary>
        /// <param name="livre"></param>
        private void RemplirModifCommandeLivre(CommandeDocumentLivre livre)
        {
            txbCLivreIdSuivi.Text = livre.Id;
            string etat = livre.Etat;
            cmbCLivreEtapeSuivi.SelectedIndex = cmbCLivreEtapeSuivi.FindStringExact(etat);
        }

        /// <summary>
        /// Vide les informations de la GroupBox d'ajout
        /// </summary>
        private void ViderAjouterCommandeLivre()
        {
            txbCLivreIdCommande.Text = "";
            nudCLivreMontantCommande.Value = 0;
            dtpCLivreDateCommande.Value = DateTime.Now;
            cbxCLivreLivreCommande.SelectedIndex = 0;
            nupCLivreExemplaireCommande.Value = 0;
        }

        /// <summary>
        /// Permet d'ajouter une commande si les champs sont saisis et que l'id de la commande n'existe pas déja
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAjoutCompleteLivres_Click(object sender, EventArgs e)
        {
            if (txbCLivreIdCommande.Text == "" || nudCLivreMontantCommande.Value < 1 || nupCLivreExemplaireCommande.Value < 1 || cbxCLivreLivreCommande.SelectedIndex == -1)
            {
                MessageBox.Show("Tout les champs doivent être remplis", "Erreur");
                return;
            }
            string idCommande = txbCLivreIdCommande.Text;
            int existLivre = bdgCommandesListeLivres.IndexOf(bdgCommandesListeLivres.List.OfType<CommandeDocumentLivre>().ToList().Find(f => f.Id == idCommande));
            int existDVD = bdgCommandesListeDvd.IndexOf(bdgCommandesListeDvd.List.OfType<CommandeDocumentDvd>().ToList().Find(f => f.Id == idCommande));
            int existRevue = bdgAbonnementsListeRevues.IndexOf(bdgAbonnementsListeRevues.List.OfType<AbonnementRevue>().ToList().Find(f => f.Id == idCommande));
            if (existDVD != -1 || existLivre != -1 || existRevue != -1)
            {
                MessageBox.Show("L'Id correspondant a cette commande existe déja", "Erreur");
                return;
            }
            int montant = (int)nudCLivreMontantCommande.Value;
            DateTime dateCommande = dtpCLivreDateCommande.Value;
            string livreId = ((Livre)bdgLivresListe.List[bdgLivresListe.Position]).Id;
            int nbExemplaires = (int)nupCLivreExemplaireCommande.Value;

            bool resultat = controle.CreerCommandeLivreDVD(idCommande, montant, dateCommande, livreId, nbExemplaires);
            lesCommandesLivre = controle.GetCommandesLivres();
            bdgCommandesListeLivres.DataSource = controle.GetCommandesLivres();
            InitDataGridViewLivreCommande();
            ajoutCommandeLivre = false;
            grpAjoutCLivres.Enabled = false;
            ViderAjouterCommandeLivre();

            if (!resultat)
            {
                MessageBox.Show("Une erreur est survenue", "Erreur");
            }
        }
        /// <summary>
        /// Disable la GroupBox d'ajout et met le boolean sur false
        /// Reinitialise les champs de la GroupBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnulerAjoutCommandLivre_Click(object sender, EventArgs e)
        {
            ajoutCommandeLivre = false;
            grpAjoutCLivres.Enabled = false;
            ViderAjouterCommandeLivre();
        }

        /// <summary>
        /// Trie la grid quand on click sur un header de la DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvLivresListeCommande_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresCommandeInfos();
            string titreColonne = dgvLivresListeCommande.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocumentLivre> sortedList = new List<CommandeDocumentLivre>();
            lesCommandesLivre = controle.GetCommandesLivres();
            switch (titreColonne)
            {
                case "DateDeCommande":
                    sortedList = lesCommandesLivre.OrderByDescending(o => o.DateDeCommande).ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandesLivre.OrderByDescending(o => o.Montant).ToList();
                    break;
                case "NombreExemplaire":
                    sortedList = lesCommandesLivre.OrderByDescending(o => o.NombreExemplaire).ToList();
                    break;
                case "Etat":
                    sortedList = lesCommandesLivre.OrderByDescending(o => o.Etat).ToList();
                    break;
            }
            InitDataGridViewRechercheLivreCommande(sortedList);
        }
        #endregion

        #region Gestion de DVDs
        /// <summary>
        /// Quand on arrive sur l'onglet
        /// 
        /// Initialise la DataGridView
        /// Remplit les ComboBox correspondantes aux livres dans Ajouter et aux differentes etapes de suivi dans Modifier
        /// Disable les GroupBox d'ajout et de modification des commandes et met les booleans a false
        /// Vide le contenu de la zone de modification au cas ou l'utilisateur a quitter l'onglet en pleine modification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabGestionCmdDvd_Enter(object sender, EventArgs e)
        {
            lesCommandesLivre = controle.GetCommandesLivres();
            bdgCommandesListeLivres.DataSource = lesCommandesLivre;
            lesAbonnementsRevues = controle.GetAbonnementsRevues();
            bdgAbonnementsListeRevues.DataSource = lesAbonnementsRevues;
            lesCommandesDvd = controle.GetCommandesDvd();
            InitDataGridViewDVDCommande();
            RemplirComboBoxDVDCommande();
            BloquerAjoutModif();
            ViderEditCommandeDVD();
            ViderAjouterCommandeDVD();
        }

        /// <summary>
        /// Remplit les ComboBox des GroupBox
        /// </summary>
        private void RemplirComboBoxDVDCommande()
        {
            List<Dvd> dvds = controle.GetAllDvd();
            bdgDvdListe.DataSource = dvds;
            cbxDVDCommande.DataSource = bdgDvdListe;
            if (cbxDVDCommande.Items.Count > 0)
            {
                cbxDVDCommande.SelectedIndex = 0;
            }

            List<Suivi> suivis = controle.GetAllSuivis();
            bdgSuivisListe.DataSource = suivis;
            cbxSuiviDVDCommande.DataSource = bdgSuivisListe;
            if (cbxSuiviDVDCommande.Items.Count > 0)
            {
                cbxSuiviDVDCommande.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Remplit la DataGridView et masque des colonnes
        /// </summary>
        private void InitDataGridViewDVDCommande()
        {
            List<CommandeDocumentDvd> dvd = controle.GetCommandesDvd();
            bdgCommandesListeDvd.DataSource = dvd;
            dgvDVDListeCommande.DataSource = bdgCommandesListeDvd;
            dgvDVDListeCommande.Columns["Id"].Visible = false;
            dgvDVDListeCommande.Columns["IdLivDVD"].Visible = false;
            dgvDVDListeCommande.Columns["idSuivi"].Visible = false;
            dgvDVDListeCommande.Columns["Duree"].Visible = false;
            dgvDVDListeCommande.Columns["Titre"].Visible = false;
            dgvDVDListeCommande.Columns["Realisateur"].Visible = false;
            dgvDVDListeCommande.Columns["Synopsis"].Visible = false;
            dgvDVDListeCommande.Columns["Genre"].Visible = false;
            dgvDVDListeCommande.Columns["Public"].Visible = false;
            dgvDVDListeCommande.Columns["Rayon"].Visible = false;
            dgvDVDListeCommande.Columns["image"].Visible = false;

            dgvDVDListeCommande.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Remplit la DataGridView et masque des colonnes apres la recherche (DVD)
        /// </summary>
        private void InitDataGridViewRechercheDVDCommande(List<CommandeDocumentDvd> dvd)
        {
            bdgCommandesListeDvd.DataSource = dvd;
            dgvDVDListeCommande.DataSource = bdgCommandesListeDvd;
            dgvDVDListeCommande.Columns["Id"].Visible = false;
            dgvDVDListeCommande.Columns["IdLivDVD"].Visible = false;
            dgvDVDListeCommande.Columns["idSuivi"].Visible = false;
            dgvDVDListeCommande.Columns["Duree"].Visible = false;
            dgvDVDListeCommande.Columns["Titre"].Visible = false;
            dgvDVDListeCommande.Columns["Realisateur"].Visible = false;
            dgvDVDListeCommande.Columns["Synopsis"].Visible = false;
            dgvDVDListeCommande.Columns["Genre"].Visible = false;
            dgvDVDListeCommande.Columns["Public"].Visible = false;
            dgvDVDListeCommande.Columns["Rayon"].Visible = false;
            dgvDVDListeCommande.Columns["image"].Visible = false;

            dgvDVDListeCommande.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Bloque les GroupeBox et set les booleans a false
        /// </summary>
        private void BloquerAjtModifCommandeDVD()
        {
            grpAjoutCommandeDVD.Enabled = false;
            grpModifCommandeDVD.Enabled = false;
            ajoutCommandeDVD = false;
            modifCommandeDVD = false;
        }

        /// <summary>
        /// Permet de trouver toutes les commandes pour un id de DVD correspondant
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RechercherNumeroCommandeDVD_Click(object sender, EventArgs e)
        {
            if (!txbDVDNumeroRechercheCommande.Text.Equals(""))
            {
                List<CommandeDocumentDvd> dvd = lesCommandesDvd.FindAll(x => x.IdLivDVD.Equals(txbDVDNumeroRechercheCommande.Text));
                txbDVDNumeroRechercheCommande.Text = "";
                if (dvd.Any())
                {
                    InitDataGridViewRechercheDVDCommande(dvd);
                }
                else
                {
                    MessageBox.Show("Numéro introuvable");
                    InitDataGridViewDVDCommande();
                }
            }
            else
            {
                InitDataGridViewDVDCommande();
            }
        }

        /// <summary>
        /// Quand la selection de ligne est changée (marche aussi quand on arrive sur la page)
        /// Les informations du livre sont remplies dans les champs correspondants
        /// 
        /// Si aucune ligne n'est selectionnée, on vide tout les champs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDVDListeCommande_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDVDListeCommande.CurrentCell != null)
            {
                try
                {
                    CommandeDocumentDvd commandeSelec = (CommandeDocumentDvd)bdgCommandesListeDvd.List[bdgCommandesListeDvd.Position];
                    AfficheDVDCommandeInfos(commandeSelec);
                    if (modifCommandeDVD)
                    {
                        RemplirModifCommandeDVD(commandeSelec);
                    }
                }
                catch
                {
                    txbDVDNumeroRechercheCommande.Text = "";
                }
            }
            else
            {
                VideDVDCommandeInfos();
            }
        }
        /// <summary>
        /// Affiche les infos du dvd envoyé dans les champs
        /// </summary>
        /// <param name="livre"></param>
        private void AfficheDVDCommandeInfos(CommandeDocumentDvd DVD)
        {
            string image = DVD.Image;
            txbDVDNumeroCommande.Text = DVD.IdLivDVD;
            txbDVDdureeCommande.Text = DVD.Duree.ToString();
            txbDVDTitreCommande.Text = DVD.Titre;
            txbDVDRealisateurCommande.Text = DVD.Realisateur;
            txbDVDSynopsisCommande.Text = DVD.Synopsis;
            txbDVDPublicCommande.Text = DVD.Public;
            txbDVDRayonCommande.Text = DVD.Rayon;
            txbDVDImageCommande.Text = image;
            try
            {
                pcbDVDImageCommande.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDVDImageCommande.Image = null;
            }
        }

        /// <summary>
        /// Vide tout les champs correspondant au DVD
        /// </summary>
        private void VideDVDCommandeInfos()
        {
            txbDVDNumeroCommande.Text = "";
            txbDVDdureeCommande.Text = "";
            txbDVDTitreCommande.Text = "";
            txbDVDRealisateurCommande.Text = "";
            txbDVDSynopsisCommande.Text = "";
            txbDVDPublicCommande.Text = "";
            txbDVDRayonCommande.Text = "";
            txbDVDImageCommande.Text = "";
            pcbDVDImageCommande.Image = null;
        }

        /// <summary>
        /// Active la GroupBox correspondante et met le boolean a true
        /// 
        /// Si une commande est en cours de modif, elle desactive et vide la GroupBox de modification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAjoutCommandeDVD_Click(object sender, EventArgs e)
        {
            ajoutCommandeDVD = true;
            grpAjoutCommandeDVD.Enabled = true;

            if (modifCommandeDVD)
            {
                grpModifCommandeDVD.Enabled = false;
                modifCommandeDVD = false;
                ViderEditCommandeDVD();
            }
        }

        /// <summary>
        /// Vide le contenu de la GroupBox d'édition
        /// </summary>
        private void ViderEditCommandeDVD()
        {
            txtCommandeIdCommandeDVDModif.Text = "";
            cbxSuiviDVDCommande.SelectedIndex = 0;
        }

        /// <summary>
        /// Si aucune ligne n'est séléctionnée, l'utilisateur en est informé
        /// 
        /// Active la GroupBox correspondante et met le boolean sur true
        /// Si un ajout étais en cours, la GroupBox est vidée et disabled
        /// 
        /// Remplit la zone de modification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifierCommandeDVD_Click(object sender, EventArgs e)
        {
            if (dgvDVDListeCommande.CurrentCell != null)
            {
                CommandeDocumentDvd laCommande = (CommandeDocumentDvd)bdgCommandesListeDvd.List[bdgCommandesListeDvd.Position];

                if (laCommande.Etat == "Réglée.")
                {
                    MessageBox.Show("Cette commande est déja réglée, impossible de retourner en arrière", "Erreur");
                    return;
                }

                grpModifCommandeDVD.Enabled = true;
                modifCommandeDVD = true;

                if (ajoutCommandeDVD)
                {
                    grpAjoutCommandeDVD.Enabled = false;
                    ajoutCommandeDVD = false;
                    ViderAjouterCommandeDVD();
                }

                RemplirModifCommandeDVD(laCommande);
            }
            else
            {
                MessageBox.Show("Veuillez selectionner une commande", "Erreur");
            }
        }

        /// <summary>
        /// Remplit les champs de la GroupBox d'édition avec les valeurs du dvd
        /// </summary>
        /// <param name="dvd"></param>
        private void RemplirModifCommandeDVD(CommandeDocumentDvd dvd)
        {
            txtCommandeIdCommandeDVDModif.Text = dvd.Id;
            string etat = dvd.Etat;
            cbxSuiviDVDCommande.SelectedIndex = cbxSuiviDVDCommande.FindStringExact(etat);
        }

        /// <summary>
        /// Vide les informations de la GroupBox d'ajout
        /// </summary>
        private void ViderAjouterCommandeDVD()
        {
            txtDVDIdCommandeAjout.Text = "";
            nbMontantDVDCommande.Value = 0;
            dateDVDCommande.Value = DateTime.Now;
            cbxDVDCommande.SelectedIndex = 0;
            nbExemplairesDVDCommande.Value = 0;
        }

        /// <summary>
        /// Permet d'ajouter une commande si les champs sont saisis et que l'id de la commande n'existe pas déja
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAjoutCompleteDVD_Click(object sender, EventArgs e)
        {
            if (txtDVDIdCommandeAjout.Text == "" || nbMontantDVDCommande.Value < 1 || nbExemplairesDVDCommande.Value < 1 || cbxDVDCommande.SelectedIndex == -1)
            {
                MessageBox.Show("Tout les champs doivent être remplis", "Erreur");
                return;
            }
            string idCommande = txtDVDIdCommandeAjout.Text;
            int existLivre = bdgCommandesListeLivres.IndexOf(bdgCommandesListeLivres.List.OfType<CommandeDocumentLivre>().ToList().Find(f => f.Id == idCommande));
            int existDVD = bdgCommandesListeDvd.IndexOf(bdgCommandesListeDvd.List.OfType<CommandeDocumentDvd>().ToList().Find(f => f.Id == idCommande));
            int existRevue = bdgAbonnementsListeRevues.IndexOf(bdgAbonnementsListeRevues.List.OfType<AbonnementRevue>().ToList().Find(f => f.Id == idCommande));
            if (existDVD != -1 || existLivre != -1 || existRevue != -1)
            {
                MessageBox.Show("L'Id correspondant a cette commande existe déja", "Erreur");
                return;
            }
            int montant = (int)nbMontantDVDCommande.Value;
            DateTime dateCommande = dateDVDCommande.Value;
            string livreId = ((Dvd)bdgDvdListe.List[bdgDvdListe.Position]).Id;
            int nbExemplaires = (int)nbExemplairesDVDCommande.Value;

            bool resultat = controle.CreerCommandeLivreDVD(idCommande, montant, dateCommande, livreId, nbExemplaires);
            lesCommandesDvd = controle.GetCommandesDvd();
            InitDataGridViewDVDCommande();
            ajoutCommandeDVD = false;
            grpAjoutCommandeDVD.Enabled = false;
            ViderAjouterCommandeDVD();

            if (!resultat)
            {
                MessageBox.Show("Une erreur est survenue", "Erreur");
            }
        }

        /// <summary>
        /// Disable la GroupBox d'ajout et met le boolean sur false
        /// Reinitialise les champs de la GroupBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnulerAjoutCommandDVD_Click(object sender, EventArgs e)
        {
            ajoutCommandeDVD = false;
            grpAjoutCommandeDVD.Enabled = false;
            ViderAjouterCommandeDVD();
        }

        /// <summary>
        /// Si la commande est dans un état trop avancé (Livré ou Réglé) on ne peut pas selectionner changer la valeur a "En cours." ou "Rélancée."
        /// L'état d'une commande ne peut etre changer sur livrée seulement si la commande a été réglée avant
        /// 
        /// Update les valeurs dans la BDD et actualise la DataGridView
        /// Disable le groupe et vide tout les champs
        /// Met le boolean a false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifCompleteDVD_Click(object sender, EventArgs e)
        {
            Suivi suivi = (Suivi)bdgSuivisListe.List[bdgSuivisListe.Position];
            string suiviId = suivi.Id;
            string suiviLibelle = suivi.Libelle;
            CommandeDocumentDvd laCommande = (CommandeDocumentDvd)bdgCommandesListeDvd.List[bdgCommandesListeDvd.Position];
            if ((laCommande.Etat == "Réglée." || laCommande.Etat == "Livrée.") && ((suiviLibelle == "En cours.") || (suiviLibelle == "Relancée.")))
            {
                MessageBox.Show("La commande est dans un stade trop avancé pour revenir a cet état", "Erreur");
                RemplirModifCommandeDVD(laCommande);
                return;
            }
            if (suiviLibelle == "Réglée." && laCommande.Etat != "Livrée.")
            {
                MessageBox.Show("La commande ne peut etre réglée sans être livrée avant.", "Erreur");
                RemplirModifCommandeDVD(laCommande);
                return;
            }

            lesCommandesDvd = controle.GetCommandesDvd();
            bdgCommandesListeDvd.DataSource = controle.GetCommandesDvd();
            controle.ModifierCommandeLivreDVD(laCommande.Id, suiviId);
            InitDataGridViewDVDCommande();
            ViderEditCommandeDVD();

            modifCommandeDVD = false;
            grpModifCommandeDVD.Enabled = false;
        }

        /// <summary>
        /// Si on annule l'edition d'une commande, les infos dans les groupe sont vidées, le groupe est disable et le boolean est set à false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnulerEditCommandDVD_Click(object sender, EventArgs e)
        {
            ViderEditCommandeDVD();
            modifCommandeDVD = false;
            grpModifCommandeDVD.Enabled = false;
        }

        /// <summary>
        /// Supprime la commande séléctionée dans la BDD après acceptation par l'utilisateur et actualise la DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprimerCommandeDVD_Click(object sender, EventArgs e)
        {
            if (dgvDVDListeCommande.CurrentCell != null)
            {
                CommandeDocumentDvd laCommande = (CommandeDocumentDvd)bdgCommandesListeDvd.List[bdgCommandesListeDvd.Position];
                if (laCommande.Etat == "Livrée." || laCommande.Etat == "Réglée.")
                {
                    MessageBox.Show("La commande est dans un stade trop avancé pour être supprimée");
                    return;
                }
                DialogResult dialogResult = MessageBox.Show($"Êtes vous sur(e) de vouloir supprimer la commande ayant pour id : {laCommande.Id}", "Confirmer", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    bool resultat = controle.SupprimerCommandeLivreDVD(laCommande.Id);
                    InitDataGridViewDVDCommande();
                    if (!resultat)
                    {
                        MessageBox.Show("Une erreur est survenue", "Erreur");
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    InitDataGridViewDVDCommande();
                }
            }
            else
            {
                MessageBox.Show("Veuillez selectionner une commande", "Erreur");
            }
        }

        /// <summary>
        /// Trie la grid quand on click sur un header de la DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDVDListeCommande_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDVDCommandeInfos();
            string titreColonne = dgvLivresListeCommande.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocumentDvd> sortedList = new List<CommandeDocumentDvd>();
            lesCommandesDvd = controle.GetCommandesDvd();
            switch (titreColonne)
            {
                case "DateDeCommande":
                    sortedList = lesCommandesDvd.OrderByDescending(o => o.DateDeCommande).ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandesDvd.OrderByDescending(o => o.Montant).ToList();
                    break;
                case "NombreExemplaire":
                    sortedList = lesCommandesDvd.OrderByDescending(o => o.NombreExemplaire).ToList();
                    break;
                case "Etat":
                    sortedList = lesCommandesDvd.OrderByDescending(o => o.Etat).ToList();
                    break;
            }
            InitDataGridViewRechercheDVDCommande(sortedList);
        }
        #endregion

        #region Commande de Revues
        /// <summary>
        /// Quand on arrive sur le tab
        /// 
        /// - Setup toutes les listes et BDD afin de faire des checks dessus (comme voir si l'ID commande existe déja)
        /// - Remplit la grid
        /// - Remplit la ComboBox
        /// - Vide la GroupBox d'ajout
        /// - Appelle la methode BloquerAjoutModif qui permet de disable toutes les group box de Gestion si on quitte l'onglet
        /// en pleine modif ou ajout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabGestionCmdRevues_Enter(object sender, EventArgs e)
        {
            lesAbonnementsRevues = controle.GetAbonnementsRevues();
            lesCommandesLivre = controle.GetCommandesLivres();
            bdgCommandesListeLivres.DataSource = lesCommandesLivre;
            lesCommandesDvd = controle.GetCommandesDvd();
            bdgCommandesListeDvd.DataSource = lesCommandesDvd;
            InitDataGridViewRevueAbonnement();
            RemplirComboBoxRevueAbonnement();
            VideAjtAbonnementInfos();
            BloquerAjoutModif();
        }

        /// <summary>
        /// Permet de faire une recherche que tout les abonnements pour un numero de revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRechercheRevue_Click(object sender, EventArgs e)
        {
            if (txbNumRevue.Text != "")
            {
                List<AbonnementRevue> revue = lesAbonnementsRevues.FindAll(x => x.IdRevue.Equals(txbNumRevue.Text));
                txbNumRevue.Text = "";
                if (revue.Any())
                {
                    InitDataGridViewRechercheRevueCommande(revue);
                }
                else
                {
                    MessageBox.Show("Numéro introuvable");
                    InitDataGridViewRevueAbonnement();
                }
            }
            else
            {
                InitDataGridViewRevueAbonnement();
            }
        }

        /// <summary>
        /// Remplit la grid avec tout les abonnements
        /// </summary>
        private void InitDataGridViewRevueAbonnement()
        {
            List<AbonnementRevue> abonnements = controle.GetAbonnementsRevues();
            bdgAbonnementsListeRevues.DataSource = abonnements;
            dgvRevues.DataSource = bdgAbonnementsListeRevues;
            dgvRevues.Columns["Id"].Visible = false;
            dgvRevues.Columns["IdRevue"].Visible = false;
            dgvRevues.Columns["Empruntable"].Visible = false;
            dgvRevues.Columns["Titre"].Visible = false;
            dgvRevues.Columns["Periodicite"].Visible = false;
            dgvRevues.Columns["DelaiMiseDispo"].Visible = false;
            dgvRevues.Columns["Genre"].Visible = false;
            dgvRevues.Columns["Public"].Visible = false;
            dgvRevues.Columns["Rayon"].Visible = false;
            dgvRevues.Columns["Image"].Visible = false;

            dgvRevues.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Remplit la grid apres recheche
        /// </summary>
        /// <param name="abonnements"></param>
        private void InitDataGridViewRechercheRevueCommande(List<AbonnementRevue> abonnements)
        {
            bdgAbonnementsListeRevues.DataSource = abonnements;
            dgvRevues.DataSource = bdgAbonnementsListeRevues;
            dgvRevues.Columns["Id"].Visible = false;
            dgvRevues.Columns["IdRevue"].Visible = false;
            dgvRevues.Columns["Empruntable"].Visible = false;
            dgvRevues.Columns["Titre"].Visible = false;
            dgvRevues.Columns["Periodicite"].Visible = false;
            dgvRevues.Columns["DelaiMiseDispo"].Visible = false;
            dgvRevues.Columns["Genre"].Visible = false;
            dgvRevues.Columns["Public"].Visible = false;
            dgvRevues.Columns["Rayon"].Visible = false;
            dgvRevues.Columns["Image"].Visible = false;

            dgvRevues.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Rempli les champs correspondants a la ligne selectionnée de la DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevues_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevues.CurrentCell != null)
            {
                try
                {
                    AbonnementRevue abonnementSelec = (AbonnementRevue)bdgAbonnementsListeRevues.List[bdgAbonnementsListeRevues.Position];
                    AfficheRevueCommandeInfos(abonnementSelec);
                }
                catch
                {
                    txbNumRevue.Text = "";
                }
            }
            else
            {
                VideRevueAbonnementInfos();
            }
        }
        /// <summary>
        /// Rempli les champs correspondants a la ligne selectionnée de la DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AfficheRevueCommandeInfos(AbonnementRevue revue)
        {
            txbNumeroRevue.Text = revue.IdRevue;
            cbRevue.Checked = revue.Empruntable;
            txbDelaiRevue.Text = revue.DelaiMiseDispo.ToString();
            txbPeriodiciteRevue.Text = revue.Periodicite;
            txbGenreRevue.Text = revue.Genre;
            txbPublicRevue.Text = revue.Public;
            txbRayonRevue.Text = revue.Rayon;
            txbTitreRevue.Text = revue.Titre;
            string image = revue.Image;
            txbRevuesImage.Text = image;
            try
            {
                picRevue.Image = Image.FromFile(image);
            }
            catch
            {
                picRevue.Image = null;
            }
        }

        /// <summary>
        /// Vide tout les champs lié aux abonnements
        /// </summary>
        private void VideRevueAbonnementInfos()
        {
            txbNumeroRevue.Text = "";
            cbRevue.Checked = false;
            txbTitreRevue.Text = "";
            txbPeriodiciteRevue.Text = "";
            txbDelaiRevue.Text = "";
            txbGenreRevue.Text = "";
            txbPublicRevue.Text = "";
            txbRayonRevue.Text = "";
            txbImgRevue.Text = "";
            picRevue.Image = null;
        }

        /// <summary>
        /// Vide la GroupBox d'ajout
        /// </summary>
        private void VideAjtAbonnementInfos()
        {
            txbIdCommande.Text = "";
            cbxRevue.SelectedIndex = 0;
            montantRevue.Value = 0;
            dtpDebutRevue.Value = DateTime.Now;
            dptFinRevue.Value = DateTime.Now;
        }

        /// <summary>
        /// Remplit la ComboBox avec les revues
        /// </summary>
        private void RemplirComboBoxRevueAbonnement()
        {
            List<Revue> revues = controle.GetAllRevues();
            bdgRevuesListe.DataSource = revues;
            cbxRevue.DataSource = bdgRevuesListe;
            if (cbxRevue.Items.Count > 0)
            {
                cbxRevue.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Enable la GroupBox d'ajout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAjtRevue_Click(object sender, EventArgs e)
        {
            grpAjtAbonnement.Enabled = true;
        }

        /// <summary>
        /// Disable la GroupBox d'ajout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnulAjtRevue_Click(object sender, EventArgs e)
        {
            grpAjtAbonnement.Enabled = false;
            VideAjtAbonnementInfos();
        }

        /// <summary>
        /// Creer un nouvel abonnement si l'id commande n'existe pas et que tout les champs sont remplis
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAjtFiniRevue_Click(object sender, EventArgs e)
        {
            if (txbIdCommande.Text == "" || cbxRevue.SelectedIndex == -1 || montantRevue.Value < 1)
            {
                MessageBox.Show("Tout les champs doivent etre remplis", "Erreur");
                return;
            }
            if (dtpDebutRevue.Value < DateTime.Today || dptFinRevue.Value < DateTime.Today)
            {
                MessageBox.Show("L'une des deux dates est inferieur a la date du jour", "Erreur");
                return;
            }
            if (dtpDebutRevue.Value >= dptFinRevue.Value)
            {
                MessageBox.Show("La date de debut d'abonnement est supérieure ou égale a la date de fin", "Erreur");
                return;
            }
            string idCommande = txbIdCommande.Text;
            int existLivre = bdgCommandesListeLivres.IndexOf(bdgCommandesListeLivres.List.OfType<CommandeDocumentLivre>().ToList().Find(f => f.Id == idCommande));
            int existDVD = bdgCommandesListeDvd.IndexOf(bdgCommandesListeDvd.List.OfType<CommandeDocumentDvd>().ToList().Find(f => f.Id == idCommande));
            int existRevue = bdgAbonnementsListeRevues.IndexOf(bdgAbonnementsListeRevues.List.OfType<AbonnementRevue>().ToList().Find(f => f.Id == idCommande));
            if (existDVD != -1 || existLivre != -1 || existRevue != -1)
            {
                MessageBox.Show("L'Id correspondant a cette commande existe déja", "Erreur");
                return;
            }
            int montant = (int)montantRevue.Value;
            DateTime dateDebutAbonnement = dtpDebutRevue.Value;
            DateTime dateFinAbonnement = dptFinRevue.Value;
            string revueId = ((Revue)bdgRevuesListe.List[bdgRevuesListe.Position]).Id;
            bool resultat = controle.CreerAbonnement(idCommande, montant, dateDebutAbonnement, dateFinAbonnement, revueId);
            lesAbonnementsRevues = controle.GetAbonnementsRevues();
            InitDataGridViewRevueAbonnement();
            grpAjtAbonnement.Enabled = false;
            VideAjtAbonnementInfos();

            if (!resultat)
            {
                MessageBox.Show("Une erreur est survenue", "Erreur");
            }
        }

        /// <summary>
        /// Supprime la ligne selectionnée si il n'existe pas de date d'achat d'exemplaire comprise entre les dates de l'abonnement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprRevue_Click(object sender, EventArgs e)
        {
            bool exist = false;
            AbonnementRevue abonnement = (AbonnementRevue)bdgAbonnementsListeRevues.List[bdgAbonnementsListeRevues.Position];
            List<Exemplaire> exemplaires = controle.GetExemplairesRevue(abonnement.IdRevue);

            foreach (var exemplaire in exemplaires.Where(x => ParutionDansAbonnement(abonnement.DateDeCommande, abonnement.DateDeFinAbonnement, x.DateAchat)))
            {
                exist = true;
            }

            if (exist)
            {
                MessageBox.Show("Au moins un exemplaire existe pendant la durée de l'abonnement", "Erreur");
                return;
            }

            DialogResult dialogResult = MessageBox.Show($"Êtes-vous sur(e) de vouloir supprimer l'abonnement ayant pour id : {abonnement.Id}", "Confirmer", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                bool resultat = controle.SupprimerAbonnnement(abonnement.Id);
                InitDataGridViewRevueAbonnement();

                if (!resultat)
                {
                    MessageBox.Show("Une erreur est survenue", "Erreur");
                }
            }
        }

        /// <summary>
        /// Permet de savoir si la date d'achat est comprise entre les date de debut et fin de l'abonnement
        /// </summary>
        /// <param name="dateCommande"></param>
        /// <param name="dateFinAbonnement"></param>
        /// <param name="dateParution"></param>
        /// <returns></returns>
        public bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFinAbonnement, DateTime dateParution)
        {
            return (dateParution > dateCommande && dateParution < dateFinAbonnement);
        }

        /// <summary>
        /// Trie la grid quand on click sur un header de la DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevues_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevueAbonnementInfos();
            string titreColonne = dgvRevues.Columns[e.ColumnIndex].HeaderText;
            List<AbonnementRevue> sortedList = new List<AbonnementRevue>();
            lesAbonnementsRevues = controle.GetAbonnementsRevues();
            switch (titreColonne)
            {
                case "DateDeCommande":
                    sortedList = lesAbonnementsRevues.OrderByDescending(o => o.DateDeCommande).ToList();
                    break;
                case "DateDeFinAbonnement":
                    sortedList = lesAbonnementsRevues.OrderByDescending(o => o.DateDeFinAbonnement).ToList();
                    break;
                case "Montant":
                    sortedList = lesAbonnementsRevues.OrderByDescending(o => o.Montant).ToList();
                    break;
            }
            InitDataGridViewRechercheRevueCommande(sortedList);
        }
        #endregion

    }
}
