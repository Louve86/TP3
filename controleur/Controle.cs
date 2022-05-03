using System.Collections.Generic;
using Mediatek86.modele;
using Mediatek86.metier;
using Mediatek86.vue;
using System;
using Serilog;


namespace Mediatek86.controleur
{
    public class Controle
    {
        private readonly List<Livre> lesLivres;
        private readonly List<Dvd> lesDvd;
        private readonly List<Revue> lesRevues;
        private readonly List<Categorie> lesRayons;
        private readonly List<Categorie> lesPublics;
        private readonly List<Categorie> lesGenres;
        private readonly List<Suivi> lesSuivis;

        /// <summary>
        /// Ouverture de la fenêtre
        /// </summary>
        public Controle()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/logs-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            lesLivres = Dao.GetAllLivres();
            lesDvd = Dao.GetAllDvd();
            lesRevues = Dao.GetAllRevues();
            lesGenres = Dao.GetAllGenres();
            lesRayons = Dao.GetAllRayons();
            lesPublics = Dao.GetAllPublics();
            lesSuivis = Dao.GetAllSuivis();
            ///FrmAuth frmAuth = new FrmAuth(this);
            ///frmAuth.ShowDialog();
        }

        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Collection d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return lesGenres;
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Collection d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return lesLivres;
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Collection d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return lesDvd;
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Collection d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return lesRevues;
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Collection d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return lesRayons;
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Collection d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return lesPublics;
        }

        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <returns>Collection d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocuement)
        {
            return Dao.GetExemplairesRevue(idDocuement);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return Dao.CreerExemplaire(exemplaire);
        }

        ///Lien Entre la classe CommandeDocumentLivre et la BDD
        public List<CommandeDocumentLivre> GetCommandesLivres()
        {
            List<CommandeDocumentLivre> lesCommandesLivres;
            lesCommandesLivres = Dao.GetCommandesLivres();
            return lesCommandesLivres;            
        }
        /// <summary>
        /// Recupère tout les suivis de la base de données
        /// </summary>
        /// <returns>La liste contenant tout les suivis</returns>
        public List<Suivi> GetAllSuivis()
        {
            return lesSuivis;
        }

        /// <summary>
        /// Permet de modifier une commande depuis l'onglet de Gestion des commandes (Livres)
        /// </summary>
        /// <param name="idCommande"></param>
        /// <param name="idSuivi"></param>
        public bool ModifierCommandeLivreDVD(string idCommande, string idSuivi)
        {
            bool resultat = Dao.ModifierCommandeLivreDVD(idCommande, idSuivi);
            return resultat;
        }

        /// <summary>
        /// Permet de créer une commande depuis l'onglet de Gestion des commandes (Livres)
        /// </summary>
        /// <param name="idCommande"></param>
        /// <param name="montant"></param>
        /// <param name="dateCommande"></param>
        /// <param name="livreId"></param>
        /// <param name="nbExemplaire"></param>
        public bool CreerCommandeLivreDVD(string idCommande, int montant, DateTime dateCommande, string livreId, int nbExemplaire)
        {
            bool resultat1 = Dao.CreerCommande(idCommande, montant, dateCommande);
            if (!resultat1) return false;
            bool resultat2 = Dao.CreerCommandeDocument2(idCommande, livreId, nbExemplaire);
            if (!resultat2) return false;

            return true;
        }

        /// <summary>
        /// Permet de supprimer une commande depuis l'onglet de Gestion des commandes (Livres)
        /// </summary>
        /// <param name="id"></param>
        public bool SupprimerCommandeLivreDVD(string id)
        {
            bool resultat1 = Dao.SupprimerCommande(id);
            if (!resultat1) return false;
            bool resultat2 = Dao.SupprimerCommande2(id);
            if (!resultat2) return false;

            return true;
        }

        /// <summary>
        /// Recupère toutes les commandes de DVD dans la base de données
        /// </summary>
        /// <returns>La liste contenant toutes les commandes</returns>
        public List<CommandeDocumentDvd> GetCommandesDvd()
        {
            List<CommandeDocumentDvd> lesCommandesDvd;
            lesCommandesDvd = Dao.GetCommandesDvd();
            return lesCommandesDvd;
        }

        /// <summary>
        /// Recupère tout les abonnements aux revues dans la base de données
        /// </summary>
        /// <returns>La liste contenant toutes les abonnements</returns>
        public List<AbonnementRevue> GetAbonnementsRevues()
        {
            List<AbonnementRevue> lesAbonnementsRevues;
            lesAbonnementsRevues = Dao.GetAbonnementsRevues();
            return lesAbonnementsRevues;
        }

        /// <summary>
        /// Permet de creer un abonnement
        /// </summary>
        /// <param name="idCommande"></param>
        /// <param name="montant"></param>
        /// <param name="dateDebutAbonnement"></param>
        /// <param name="dateFinAbonnement"></param>
        /// <param name="revueId"></param>
        public bool CreerAbonnement(string idCommande, int montant, DateTime dateDebutAbonnement,
            DateTime dateFinAbonnement, string revueId)
        {
            bool resultat1 = Dao.CreerCommande(idCommande, montant, dateDebutAbonnement);
            if (!resultat1) return false;
            bool resultat2 = Dao.CreerAbonnement(idCommande, dateFinAbonnement, revueId);
            if (!resultat2) return false;

            return true;
        }

        /// <summary>
        /// Permet de supprimer un abonnement
        /// </summary>
        /// <param name="idCommande"></param>
        public bool SupprimerAbonnnement(string idCommande)
        {
            bool resultat1 = Dao.SupprimerAbonnement(idCommande);
            if (!resultat1) return false;
            bool resultat2 = Dao.SupprimerCommande2(idCommande);
            if (!resultat2) return false;

            return true;
        }

        /// <summary>
        /// Permet d'afficher tout les abonnements dont la date de fin est en dessous de 30 jours au login
        /// 
        /// Ne s'execute qu'une fois au démarrage
        /// </summary>
        /// <returns>String comportant toutes les infos a afficher dans une MessageBox</returns>
        public string GetAbonnementsSub30Days()
        {
            string procedure = Dao.GetAbonnementsSub30Days();
            return procedure;
        }

        /// <summary>
        /// Check si le combo identifiant mdp est valide pour se connecter
        /// 
        /// Si oui return l'id du service
        /// Si non return null;
        /// </summary>
        /// <param name="identifiant"></param>
        /// <param name="mdp"></param>
        /*public Service ControleAuthentification(string identifiant, string mdp)
        {
            Service service = Dao.ControleAuthentification(identifiant, mdp);
            return service;
        }*/
    }
}

