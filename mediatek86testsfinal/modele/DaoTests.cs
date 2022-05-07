using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mediatek86.modele;
using Mediatek86.metier;
using Mediatek86.bdd;
using System.Collections.Generic;
using System;

namespace Mediatek86.modele.Tests
{
    [TestClass()]
    public class DaoTests
    {
        private static readonly string server = "localhost";
        private static readonly string userid = "root";
        private static readonly string password = "";
        private static readonly string database = "mediatek86";
        private static readonly string connectionString = "server=" + server + ";user id=" + userid + ";password=" + password + ";database=" + database + ";SslMode=none";
        private readonly BddMySql curs = BddMySql.GetInstance(connectionString);

        /// <summary>
        /// Permet de ne pas affecter la BDD
        /// </summary>
        private void BeginTransaction()
        {
            curs.ReqUpdate("SET AUTOCOMMIT=0", null);
        }

        /// <summary>
        /// Rollback les changements pour ne pas affecter la BDD
        /// </summary>
        private void EndTransaction()
        {
            curs.ReqUpdate("ROLLBACK", null);
        }

        /// <summary>
        /// Test unitaire sur la fonction permettant de recuperer les genres
        /// </summary>
        [TestMethod()]
        public void GetAllGenresTest()
        {
            List<Categorie> lesGenres = Dao.GetAllGenres();
            Assert.IsTrue(lesGenres.Count != 0, "devrait réussir : au moins un genre");
        }

        /// <summary>
        /// Test unitaire sur la fonction permettant de recuperer les rayons
        /// </summary>
        [TestMethod()]
        public void GetAllRayonsTest()
        {
            List<Categorie> lesRayons = Dao.GetAllRayons();
            Assert.IsTrue(lesRayons.Count != 0, "devrait réussir : au moins un rayon");
        }

        /// <summary>
        /// Test unitaire sur la fonction permettant de recuperer les publics
        /// </summary>
        [TestMethod()]
        public void GetAllPublicsTest()
        {
            List<Categorie> lesPublics = Dao.GetAllPublics();
            Assert.IsTrue(lesPublics.Count != 0, "devrait réussir : au moins un public");
        }

        /// <summary>
        /// Test unitaire sur la fonction permettant de recuperer les livres
        /// </summary>
        [TestMethod()]
        public void GetAllLivresTest()
        {
            List<Livre> lesLivres = Dao.GetAllLivres();
            Assert.IsTrue(lesLivres.Count != 0, "devrait réussir : au moins un livre");
        }

        /// <summary>
        /// Test unitaire sur la fonction permettant de recuperer les DVDs
        /// </summary>
        [TestMethod()]
        public void GetAllDvdTest()
        {
            List<Dvd> lesDVDs = Dao.GetAllDvd();
            Assert.IsTrue(lesDVDs.Count != 0, "devrait réussir : au moins un DVD");
        }

        /// <summary>
        /// Test unitaire sur la fonction permettant de recuperer les Revues
        /// </summary>
        [TestMethod()]
        public void GetAllRevuesTest()
        {
            List<Revue> lesRevues = Dao.GetAllRevues();
            Assert.IsTrue(lesRevues.Count != 0, "devrait réussir : au moins une revue");
        }

        /// <summary>
        /// Test unitaire sur la fonction permettant de recuperer l'exemplaire correspondant a un ID donné
        /// </summary>
        [TestMethod()]
        public void GetExemplairesRevueTest()
        {
            List<Exemplaire> lesExemplaires = Dao.GetExemplairesRevue("10011");
            Assert.IsTrue(lesExemplaires.Count != 0, "devrait réussir : au moins un exemplaire de cette revue existe");
        }

        [TestMethod()]
        public void CreerExemplaireTest()
        {
            BeginTransaction();
            List<Exemplaire> lesExemplaires = Dao.GetExemplairesRevue("00001");
            int nbBeforeInsert = lesExemplaires.Count;
            int numero = 99999;
            DateTime dateAchat = DateTime.Today;
            string photo = "";
            string idEtat = "00001";
            string idDocument = "00001";
            Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
            Dao.CreerExemplaire(exemplaire);
            List<Exemplaire> lesExemplairesAfter = Dao.GetExemplairesRevue("00001");
            int nbAfterInsert = lesExemplairesAfter.Count;
            Exemplaire exemplaireAfter = lesExemplairesAfter.Find(obj => obj.Numero.Equals(numero)
                && (obj.DateAchat == dateAchat)
                && (obj.Photo == photo)
                && (obj.IdEtat == idEtat)
                && obj.IdDocument.Equals(idDocument)
                );
            Assert.IsNotNull(exemplaireAfter, "devrait réussir : un exemplaire a été ajouté");
            Assert.AreEqual(nbBeforeInsert + 1, nbAfterInsert, "devrait réussir : un exemplaire en plus");
            EndTransaction();
        }

        /// <summary>
        /// Test unitaire sur la fonction permettant de recuperer les commandes de Livres/DVDs
        /// </summary>
        [TestMethod()]
        public void GetCommandesLivresTest()
        {
            List<CommandeDocumentLivre> lesCommandesLivres = Dao.GetCommandesLivres();
            Assert.IsTrue(lesCommandesLivres.Count != 0, "devrait réussir : au moins une commande de livre");
        }

        /// <summary>
        /// Test unitaire sur la fonction permettant de recuperer tout les suivis
        /// </summary>
        [TestMethod()]
        public void GetAllSuivisTest()
        {
            List<Suivi> lesSuivis = Dao.GetAllSuivis();
            Assert.IsTrue(lesSuivis.Count != 0, "devrait réussir : au moins une etape de suivi existe");
        }

        /// <summary>
        /// Test unitaire sur la fonction permettant de verifier que la methode de modification de Livres/DVDs fonctionne
        /// </summary>
        [TestMethod()]
        public void ModifierCommandeLivreDVDTest()
        {
            BeginTransaction();
            // Test identique sur commande DVD, donc je vais uniquement faire le test sur les livres
            List<CommandeDocumentLivre> lesCommandesLivres = Dao.GetCommandesLivres();
            int nbBeforeUpdate = lesCommandesLivres.Count;
            if (nbBeforeUpdate > 0)
            {
                string id = "1";
                DateTime date = DateTime.Parse("2022-03-09");
                double montant = 1;
                int nbExemplaires = 1;
                string idLivre = "00001";
                string idSuivi = "00001";
                string libelleSuivi = "En cours.";
                string isbn = "1234569877896";
                string titre = "Quand sort la recluse";
                string auteur = "Fred Vargas";
                string collection = "Commissaire Adamsberg";
                string genre = "Policier";
                string publicdoc = "Adultes";
                string rayon = "Policiers français étrangers";
                string image = "";

                Dao.ModifierCommandeLivreDVD(id, idSuivi);
                lesCommandesLivres = Dao.GetCommandesLivres();
                int nbAfterUpdate = lesCommandesLivres.Count;
                CommandeDocumentLivre commandeModif = lesCommandesLivres.Find(obj => obj.Id.Equals(id));

                if (commandeModif != null)
                {
                    bool identique = (commandeModif.DateDeCommande == date)
                        && (commandeModif.Montant == montant + "€")
                        && (commandeModif.NombreExemplaire == nbExemplaires)
                        && commandeModif.IdLivDVD.Equals(idLivre)
                        && commandeModif.IdSuivi.Equals(idSuivi)
                        && commandeModif.Etat.Equals(libelleSuivi)
                        && commandeModif.ISBN.Equals(isbn)
                        && commandeModif.Titre.Equals(titre)
                        && commandeModif.Auteur.Equals(auteur)
                        && commandeModif.Collection.Equals(collection)
                        && commandeModif.Genre.Equals(genre)
                        && commandeModif.Public.Equals(publicdoc)
                        && commandeModif.Rayon.Equals(rayon)
                        && commandeModif.Image.Equals(image);
                    Assert.AreEqual(true, identique, "devrait reussir : une commande a étée modifiée");
                }
                else
                {
                    Assert.Fail("Commande perdue suite aux modifications");
                }
                Assert.AreEqual(nbBeforeUpdate, nbAfterUpdate, "devrait réussir : autant de commandes");
            }
            EndTransaction();
        }

        /// <summary>
        /// Test unitaire sur la fonction permettant de verifier que la methode de création de commande fonctionne
        /// </summary>
        [TestMethod()]
        public void CreerCommandeTest()
        {
            BeginTransaction();
            // Je ne ferrais le test que sur les commandes de livres meme si cette methode est universelle
            // Car si ca fonctionne pour l'un cela fonctionnera pour les deux autres
            List<CommandeDocumentLivre> lesCommandesLivres = Dao.GetCommandesLivres();
            int nbBeforeInsert = lesCommandesLivres.Count;
            string id = "TESTS";
            DateTime date = DateTime.Today;
            double montant = 1;
            int nbExemplaires = 1;
            string idLivre = "00001";
            string idSuivi = "00001";
            string libelleSuivi = "En cours.";
            string isbn = "1234569877896";
            string titre = "Quand sort la recluse";
            string auteur = "Fred Vargas";
            string collection = "Commissaire Adamsberg";
            string genre = "Policier";
            string publicdoc = "Adultes";
            string rayon = "Policiers français étrangers";
            string image = "";
            Dao.CreerCommande(id, (int)montant, date);
            Dao.CreerCommandeDocument2(id, idLivre, nbExemplaires);
            lesCommandesLivres = Dao.GetCommandesLivres();
            int nbAfterInsert = lesCommandesLivres.Count;
            CommandeDocumentLivre commandeAdd = lesCommandesLivres.Find(obj => obj.Id.Equals(id)
                && (obj.DateDeCommande == date)
                && (obj.Montant == montant + "€")
                && (obj.NombreExemplaire == nbExemplaires)
                && obj.IdLivDVD.Equals(idLivre)
                && obj.IdSuivi.Equals(idSuivi)
                && obj.Etat.Equals(libelleSuivi)
                && obj.ISBN.Equals(isbn)
                && obj.Titre.Equals(titre)
                && obj.Auteur.Equals(auteur)
                && obj.Collection.Equals(collection)
                && obj.Genre.Equals(genre)
                && obj.Public.Equals(publicdoc)
                && obj.Rayon.Equals(rayon)
                && obj.Image.Equals(image)
                );
            Assert.IsNotNull(commandeAdd, "devrait réussir : une commande a étée ajouté");
            Assert.AreEqual(nbBeforeInsert + 1, nbAfterInsert, "devrait réussir : une commande en plus");
            EndTransaction();
        }

        /// <summary>
        /// Test unitaire sur la fonction permettant de verifier que la methode de création de commande fonctionne
        /// </summary>
        [TestMethod()]
        public void CreerCommandeDocument2Test()
        {
            BeginTransaction();
            // Identique a CreerCommandeDocument (Sauf que ca fonctionne que pour les Livres/DVDs
            List<CommandeDocumentLivre> lesCommandesLivres = Dao.GetCommandesLivres();
            int nbBeforeInsert = lesCommandesLivres.Count;
            string id = "TEST";
            DateTime date = DateTime.Today;
            double montant = 1;
            int nbExemplaires = 1;
            string idLivre = "00001";
            string idSuivi = "00001";
            string libelleSuivi = "En cours.";
            string isbn = "1234569877896";
            string titre = "Quand sort la recluse";
            string auteur = "Fred Vargas";
            string collection = "Commissaire Adamsberg";
            string genre = "Policier";
            string publicdoc = "Adultes";
            string rayon = "Policiers français étrangers";
            string image = "";
            Dao.CreerCommande(id, (int)montant, date);
            Dao.CreerCommandeDocument2(id, idLivre, nbExemplaires);
            lesCommandesLivres = Dao.GetCommandesLivres();
            int nbAfterInsert = lesCommandesLivres.Count;
            CommandeDocumentLivre commandeAdd = lesCommandesLivres.Find(obj => obj.Id.Equals(id)
                && (obj.DateDeCommande == date)
                && (obj.Montant == montant + "€")
                && (obj.NombreExemplaire == nbExemplaires)
                && obj.IdLivDVD.Equals(idLivre)
                && obj.IdSuivi.Equals(idSuivi)
                && obj.Etat.Equals(libelleSuivi)
                && obj.ISBN.Equals(isbn)
                && obj.Titre.Equals(titre)
                && obj.Auteur.Equals(auteur)
                && obj.Collection.Equals(collection)
                && obj.Genre.Equals(genre)
                && obj.Public.Equals(publicdoc)
                && obj.Rayon.Equals(rayon)
                && obj.Image.Equals(image)
                );
            Assert.IsNotNull(commandeAdd, "devrait réussir : une commande a étée ajouté");
            Assert.AreEqual(nbBeforeInsert + 1, nbAfterInsert, "devrait réussir : une commande en plus");
            EndTransaction();
        }

        /// <summary>
        /// Test unitaire sur la fonction permettant de verifier que la methode de suppression de commande fonctionne
        /// </summary>
        [TestMethod()]
        public void SupprimerCommandeTest()
        {
            BeginTransaction();
            //Fonctionne pour les Livres et Dvds donc je ne vais le faire que pour l'un
            List<CommandeDocumentLivre> lesCommandesLivres = Dao.GetCommandesLivres();
            int nbBeforeDelete = lesCommandesLivres.Count;
            if (nbBeforeDelete > 0)
            {
                string id = "1";
                Dao.SupprimerCommande2(id);
                Dao.SupprimerCommande(id);
                lesCommandesLivres = Dao.GetCommandesLivres();
                CommandeDocumentLivre commandeDel = lesCommandesLivres.Find(obj => obj.Id.Equals(id));
                int nbAfterDelete = lesCommandesLivres.Count;

                Assert.IsNull(commandeDel, "devrait réussir : une commande a été supprimé");
                Assert.AreEqual(nbBeforeDelete - 1, nbAfterDelete, "devrait réussir : une commande en moins");
            }
            EndTransaction();
        }

        /// <summary>
        /// Test unitaire sur la fonction permettant de verifier que la methode de suppression de commande fonctionne
        /// </summary>
        [TestMethod()]
        public void SupprimerCommande2Test()
        {
            BeginTransaction();
            //Fonctionne pour les Livres et Dvds donc je ne vais le faire que pour l'un
            List<CommandeDocumentLivre> lesCommandesLivres = Dao.GetCommandesLivres();
            int nbBeforeDelete = lesCommandesLivres.Count;
            if (nbBeforeDelete > 0)
            {
                string id = "2";
                Dao.SupprimerCommande2(id);
                Dao.SupprimerCommande(id);
                lesCommandesLivres = Dao.GetCommandesLivres();
                CommandeDocumentLivre commandeDel = lesCommandesLivres.Find(obj => obj.Id.Equals(id));
                int nbAfterDelete = lesCommandesLivres.Count;

                Assert.IsNull(commandeDel, "devrait réussir : une commande a été supprimé");
                Assert.AreEqual(nbBeforeDelete - 1, nbAfterDelete, "devrait réussir : une commande en moins");
            }
            EndTransaction();
        }

        /// <summary>
        /// Test unitaire sur la fonction permettant de recuperer les DVDs
        /// </summary>
        [TestMethod()]
        public void GetCommandesDvdTest()
        {
            List<CommandeDocumentDvd> lesCommandesDVDs = Dao.GetCommandesDvd();
            Assert.IsTrue(lesCommandesDVDs.Count != 0, "devrait réussir : au moins une commande de DVD");
        }

        /// <summary>
        /// Test unitaire sur la fonction permettant de recuperer les abonnements
        /// </summary>
        [TestMethod()]
        public void GetAbonnementsRevuesTest()
        {
            List<AbonnementRevue> lesAbonnementsRevues = Dao.GetAbonnementsRevues();
            Assert.IsTrue(lesAbonnementsRevues.Count != 0, "devrait réussir : au moins un abonnement de revue");
        }

        /// <summary>
        /// Test unitaire sur la fonction permettant de tester la methode de création d'abonnement
        /// </summary>
        [TestMethod()]
        public void CreerAbonnementTest()
        {
            BeginTransaction();
            List<AbonnementRevue> lesAbonnements = Dao.GetAbonnementsRevues();
            int nbBeforeInsert = lesAbonnements.Count;
            string id = "TESTS";
            DateTime dateCommande = DateTime.Parse("2022-03-09");
            DateTime dateFinAbonnement = DateTime.Parse("2022-07-09");
            string idRevue = "10011";
            bool empruntable = true;
            string titre = "Geo";
            string periodicite = "MS";
            int delaiMiseDispo = 52;
            string genre = "Presse Culturelle";
            string publicdoc = "Tous publics";
            string rayon = "Magazines";
            string image = "";
            double montant = 1;
            Dao.CreerCommande(id, (int)montant, dateCommande);
            Dao.CreerAbonnement(id, dateFinAbonnement, idRevue);
            lesAbonnements = Dao.GetAbonnementsRevues();
            int nbAfterInsert = lesAbonnements.Count;
            AbonnementRevue abonnementAdd = lesAbonnements.Find(obj => obj.Id.Equals(id)
                && (obj.DateDeCommande == dateCommande)
                && (obj.DateDeFinAbonnement == dateFinAbonnement)
                && obj.IdRevue.Equals(idRevue)
                && (obj.Empruntable == empruntable)
                && obj.Titre.Equals(titre)
                && obj.Periodicite.Equals(periodicite)
                && (obj.DelaiMiseDispo == delaiMiseDispo)
                && obj.Genre.Equals(genre)
                && obj.Public.Equals(publicdoc)
                && obj.Rayon.Equals(rayon)
                && obj.Image.Equals(image)
                && (obj.Montant == montant + "€")
                );
            Assert.IsNotNull(abonnementAdd, "devrait réussir : un abonnement a été ajouté");
            Assert.AreEqual(nbBeforeInsert + 1, nbAfterInsert, "devrait réussir : un abonnement en plus");
            EndTransaction();
        }

        /// <summary>
        /// Test unitaire sur la fonction permettant de tester la methode de suppression d'abonnement
        /// </summary>
        [TestMethod()]
        public void SupprimerAbonnementTest()
        {
            BeginTransaction();
            List<AbonnementRevue> lesAbonnements = Dao.GetAbonnementsRevues();
            int nbBeforeDelete = lesAbonnements.Count;
            if (nbBeforeDelete > 0)
            {
                string id = "18";
                Dao.SupprimerAbonnement(id);
                Dao.SupprimerCommande(id);
                lesAbonnements = Dao.GetAbonnementsRevues();
                AbonnementRevue abonnementDel = lesAbonnements.Find(obj => obj.Id.Equals(id));
                int nbAfterDelete = lesAbonnements.Count;

                Assert.IsNull(abonnementDel, "devrait réussir : un abonnement supprimé");
                Assert.AreEqual(nbBeforeDelete - 1, nbAfterDelete, "devrait réussir : un abonnement en moins");
            }
            EndTransaction();
        }

        /// <summary>
        /// Test unitaire sur la fonction permettant de tester la methode de connexion a l'application
        /// </summary>
        [TestMethod()]
        public void ControleAuthentificationTest()
        {
            string identifiant = "admin";
            string mdp = "admin";
            Service service = new Service("admin", 1, "Administratif");
            Service daoService = Dao.ControleAuthentification(identifiant, mdp);
            Assert.AreEqual(true, Compare(daoService, service), "devrait reussir");
            string identifiantFaux = "ananas";
            string mdpFaux = "ananas";
            Assert.AreEqual(null, Dao.ControleAuthentification(identifiantFaux, mdpFaux), "devrait echouer : tout est faux");
            Assert.AreEqual(null, Dao.ControleAuthentification(identifiant, mdpFaux), "devrait echouer : mdp est faux");
            Assert.AreEqual(null, Dao.ControleAuthentification(identifiantFaux, mdp), "devrait echouer : identifiant est faux");
        }

        /// <summary>
        /// Permet de comparer un service modele avec un autre service
        /// </summary>
        /// <param name="modele"></param>
        /// <param name="aComparer"></param>
        /// <returns>true/false</returns>
        public bool Compare(object modele, object aComparer)
        {
            Service modeleServ = (Service)modele;
            Service aComparerServ = (Service)aComparer;
            return (modeleServ.Utilisateur.Equals(aComparerServ.Utilisateur) && modeleServ.ServiceInt.Equals(aComparerServ.ServiceInt) && modeleServ.Nom.Equals(aComparerServ.Nom));
        }
    }
}
