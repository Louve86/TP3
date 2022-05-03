using Mediatek86.bdd;
using Mediatek86.metier;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Mediatek86.modele
{
    public static class Dao
    {
        private static int nb = 0;
        private static readonly string server = "localhost";
        private static readonly string userid = "root";
        private static readonly string password = "";
        private static readonly string database = "mediatek86";
        private static readonly string connectionString = "server="+server+";user id="+userid+";password="+password+";database="+database+";SslMode=none";

        /// <summary>
        /// Retourne tous les genres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public static List<Categorie> GetAllGenres()
        {
            List<Categorie> lesGenres = new List<Categorie>();
            string req = "Select * from genre order by libelle";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                Genre genre = new Genre((string)curs.Field("id"), (string)curs.Field("libelle"));
                lesGenres.Add(genre);
            }
            curs.Close();
            return lesGenres;
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Collection d'objets Rayon</returns>
        public static List<Categorie> GetAllRayons()
        {
            List<Categorie> lesRayons = new List<Categorie>();
            string req = "Select * from rayon order by libelle";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                Rayon rayon = new Rayon((string)curs.Field("id"), (string)curs.Field("libelle"));
                lesRayons.Add(rayon);
            }
            curs.Close();
            return lesRayons;
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD
        /// </summary>
        /// <returns>Collection d'objets Public</returns>
        public static List<Categorie> GetAllPublics()
        {
            List<Categorie> lesPublics = new List<Categorie>();
            string req = "Select * from public order by libelle";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                Public lePublic = new Public((string)curs.Field("id"), (string)curs.Field("libelle"));
                lesPublics.Add(lePublic);
            }
            curs.Close();
            return lesPublics;
        }

        /// <summary>
        /// Retourne toutes les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public static List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = new List<Livre>();
            string req = "Select l.id, l.ISBN, l.auteur, d.titre, d.image, l.collection, ";
            req += "d.idrayon, d.idpublic, d.idgenre, g.libelle as genre, p.libelle as public, r.libelle as rayon ";
            req += "from livre l join document d on l.id=d.id ";
            req += "join genre g on g.id=d.idGenre ";
            req += "join public p on p.id=d.idPublic ";
            req += "join rayon r on r.id=d.idRayon ";
            req += "order by titre ";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                string id = (string)curs.Field("id");
                string isbn = (string)curs.Field("ISBN");
                string auteur = (string)curs.Field("auteur");
                string titre = (string)curs.Field("titre");
                string image = (string)curs.Field("image");
                string collection = (string)curs.Field("collection");
                string idgenre = (string)curs.Field("idgenre");
                string idrayon = (string)curs.Field("idrayon");
                string idpublic = (string)curs.Field("idpublic");
                string genre = (string)curs.Field("genre");
                string lepublic = (string)curs.Field("public");
                string rayon = (string)curs.Field("rayon");
                Livre livre = new Livre(id, titre, image, isbn, auteur, collection, idgenre, genre, 
                    idpublic, lepublic, idrayon, rayon);
                lesLivres.Add(livre);
            }
            curs.Close();

            return lesLivres;
        }

        /// <summary>
        /// Retourne toutes les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public static List<Dvd> GetAllDvd()
        {
            List<Dvd> lesDvd = new List<Dvd>();
            string req = "Select l.id, l.duree, l.realisateur, d.titre, d.image, l.synopsis, ";
            req += "d.idrayon, d.idpublic, d.idgenre, g.libelle as genre, p.libelle as public, r.libelle as rayon ";
            req += "from dvd l join document d on l.id=d.id ";
            req += "join genre g on g.id=d.idGenre ";
            req += "join public p on p.id=d.idPublic ";
            req += "join rayon r on r.id=d.idRayon ";
            req += "order by titre ";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                string id = (string)curs.Field("id");
                int duree = (int)curs.Field("duree");
                string realisateur = (string)curs.Field("realisateur");
                string titre = (string)curs.Field("titre");
                string image = (string)curs.Field("image");
                string synopsis = (string)curs.Field("synopsis");
                string idgenre = (string)curs.Field("idgenre");
                string idrayon = (string)curs.Field("idrayon");
                string idpublic = (string)curs.Field("idpublic");
                string genre = (string)curs.Field("genre");
                string lepublic = (string)curs.Field("public");
                string rayon = (string)curs.Field("rayon");
                Dvd dvd = new Dvd(id, titre, image, duree, realisateur, synopsis, idgenre, genre,
                    idpublic, lepublic, idrayon, rayon);
                lesDvd.Add(dvd);
            }
            curs.Close();

            return lesDvd;
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public static List<Revue> GetAllRevues()
        {
            List<Revue> lesRevues = new List<Revue>();
            string req = "Select l.id, l.empruntable, l.periodicite, d.titre, d.image, l.delaiMiseADispo, ";
            req += "d.idrayon, d.idpublic, d.idgenre, g.libelle as genre, p.libelle as public, r.libelle as rayon ";
            req += "from revue l join document d on l.id=d.id ";
            req += "join genre g on g.id=d.idGenre ";
            req += "join public p on p.id=d.idPublic ";
            req += "join rayon r on r.id=d.idRayon ";
            req += "order by titre ";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                string id = (string)curs.Field("id");
                bool empruntable = (bool)curs.Field("empruntable");
                string periodicite = (string)curs.Field("periodicite");
                string titre = (string)curs.Field("titre");
                string image = (string)curs.Field("image");
                int delaiMiseADispo = (int)curs.Field("delaimiseadispo");
                string idgenre = (string)curs.Field("idgenre");
                string idrayon = (string)curs.Field("idrayon");
                string idpublic = (string)curs.Field("idpublic");
                string genre = (string)curs.Field("genre");
                string lepublic = (string)curs.Field("public");
                string rayon = (string)curs.Field("rayon");
                Revue revue = new Revue(id, titre, image, idgenre, genre,
                    idpublic, lepublic, idrayon, rayon, empruntable, periodicite, delaiMiseADispo);
                lesRevues.Add(revue);
            }
            curs.Close();

            return lesRevues;
        }

        /// <summary>
        /// Retourne les exemplaires d'une revue
        /// </summary>
        /// <returns>Liste d'objets Exemplaire</returns>
        public static List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            List<Exemplaire> lesExemplaires = new List<Exemplaire>();
            string req = "Select e.id, e.numero, e.dateAchat, e.photo, e.idEtat ";
            req += "from exemplaire e join document d on e.id=d.id ";
            req += "where e.id = @id ";
            req += "order by e.dateAchat DESC";
            Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@id", idDocument}
                };

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, parameters);

            while (curs.Read())
            {
                string idDocuement = (string)curs.Field("id");
                int numero = (int)curs.Field("numero");
                DateTime dateAchat = (DateTime)curs.Field("dateAchat");
                string photo = (string)curs.Field("photo");
                string idEtat = (string)curs.Field("idEtat");
                Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocuement);
                lesExemplaires.Add(exemplaire);
            }
            curs.Close();

            return lesExemplaires;
        }

        /// <summary>
        /// ecriture d'un exemplaire en base de données
        /// </summary>
        /// <param name="exemplaire"></param>
        /// <returns>true si l'insertion a pu se faire</returns>
        public static bool CreerExemplaire(Exemplaire exemplaire)
        {
            try
            {
                string req = "insert into exemplaire values (@idDocument,@numero,@dateAchat,@photo,@idEtat)";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@idDocument", exemplaire.IdDocument},
                    { "@numero", exemplaire.Numero},
                    { "@dateAchat", exemplaire.DateAchat},
                    { "@photo", exemplaire.Photo},
                    { "@idEtat",exemplaire.IdEtat}
                };
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);
                curs.Close();
                return true;
            }catch{
                return false;
            }
        }
        /// <summary>
        /// Permet d'obtenir toutes les infos liées a une commande (Livre)
        /// </summary>
        /// <returns>La List contenant toutes les infos de chaque commande (Livre)</returns>
        public static List<CommandeDocumentLivre> GetCommandesLivres()
        {
            List<CommandeDocumentLivre> lesCommandes = null;
            try
            {
                lesCommandes = new List<CommandeDocumentLivre>();
                string req = "SELECT c.id as id_commande, c.dateCommande, c.montant, cd.nbExemplaire, cd.idLivreDvd as idLivre, s.id as id_etat, s.libelle as etat, ld.ISBN, d.titre, ld.auteur, ld.collection, g.libelle as genre, p.libelle as public, r.libelle as rayon, d.image ";
                req += "FROM `commande` c ";
                req += "LEFT JOIN `commandedocument` cd USING(id) ";
                req += "LEFT JOIN `suivi` s ON s.id = cd.idSuivi ";
                req += "LEFT JOIN `document` d ON d.id = cd.idLivreDvd ";
                req += "LEFT JOIN `genre` g ON d.idGenre = g.id ";
                req += "LEFT JOIN `public` p ON d.idPublic = p.id ";
                req += "LEFT JOIN `rayon` r ON d.idRayon = r.id ";
                req += "LEFT JOIN `livre` ld ON ld.id = cd.idLivreDvd ";
                req += "WHERE cd.idLivreDvd = ld.id ";
                req += "ORDER BY c.dateCommande DESC";
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqSelect(req, null);

                while (curs.Read())
                {
                    CommandeDocumentLivre commandeDocument = new CommandeDocumentLivre(
                        (string)curs.Field("id_commande"),
                        (DateTime)curs.Field("dateCommande"),
                        (double)curs.Field("montant"),
                        (int)curs.Field("nbExemplaire"),
                        (string)curs.Field("idLivre"),
                        (string)curs.Field("id_etat"),
                        (string)curs.Field("etat"),
                        (string)curs.Field("ISBN"),
                        (string)curs.Field("titre"),
                        (string)curs.Field("auteur"),
                        (string)curs.Field("collection"),
                        (string)curs.Field("genre"),
                        (string)curs.Field("public"),
                        (string)curs.Field("rayon"),
                        (string)curs.Field("image")
                        );
                    lesCommandes.Add(commandeDocument);
                }
                curs.Close();
                return lesCommandes;
            }
            catch (Exception e)
            {
                Log.Error("Echec lors de la recupération des commandes de livres depuis la BDD\nErreur: {0}", e);
                return lesCommandes;
            }
        }
        /// <summary>
        /// Permet d'obtenir tout les suivis
        /// </summary>
        /// <returns></returns>
        public static List<Suivi> GetAllSuivis()
        {
            List<Suivi> lesSuivis = null;
            try
            {
                lesSuivis = new List<Suivi>();
                string req = "SELECT * FROM `suivi` ORDER BY libelle;";
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqSelect(req, null);

                while (curs.Read())
                {
                    Suivi suivi = new Suivi(
                        (string)curs.Field("id"),
                        (string)curs.Field("libelle")
                        );
                    lesSuivis.Add(suivi);
                }
                curs.Close();
                return lesSuivis;
            }
            catch (Exception e)
            {
                Log.Error("Echec lors de la recupération des suivis depuis la BDD\nErreur: {0}", e);
                return lesSuivis;
            }
        }

        /// <summary>
        /// Permet de modifier l'étape de suivi d'une commande (Livre)
        /// </summary>
        /// <param name="idCommande"></param>
        /// <param name="idSuivi"></param>
        public static bool ModifierCommandeLivreDVD(string idCommande, string idSuivi)
        {
            try
            {
                string req = "UPDATE commandedocument SET idSuivi = @idSuivi WHERE id = @idCommande";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@idSuivi", idSuivi },
                    { "@idCommande", idCommande}
                };
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);
                return true;
            }
            catch (Exception e)
            {
                Log.Error("Echec lors de la modification d'une commande de Livre ou de DVD\nErreur: {0}", e);
                return false;
            }
        }

        /// <summary>
        /// Permet de creer une commande dans la table commande (Universel)
        /// </summary>
        /// <param name="idCommande"></param>
        /// <param name="montant"></param>
        /// <param name="dateCommande"></param>
        public static bool CreerCommande(string idCommande, int montant, DateTime dateCommande)
        {
            try
            {
                string req = "INSERT INTO commande(id,dateCommande,montant) ";
                req += "values (@id, @dateCommande, @montant);";
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@id", idCommande);
                parameters.Add("@dateCommande", dateCommande);
                parameters.Add("@montant", montant);
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);
                return true;
            }
            catch (Exception e)
            {
                Log.Error("Echec lors de la création de la commande/abonnement dans la table commande\nErreur: {0}", e);
                return false;
            }
        }

        /// <summary>
        /// Permet de creer une commande dans la table commandedocument correspondant une commande de la table commande relié par un id (Universel)
        /// </summary>
        /// <param name="idCommande"></param>
        /// <param name="livreId"></param>
        /// <param name="nbExemplaire"></param>
        public static bool CreerCommandeDocument2(string idCommande, string livreId, int nbExemplaire)
        {
            try
            {
                string req = "INSERT INTO commandedocument(id, nbExemplaire, idLivreDvd, idSuivi) ";
                req += "values (@id, @nbExemplaire, @idLivreDvd, @idSuivi);";
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@id", idCommande);
                parameters.Add("@nbExemplaire", nbExemplaire);
                parameters.Add("@idLivreDvd", livreId);
                parameters.Add("@idSuivi", "00001");
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);
                return true;
            }
            catch (Exception e)
            {
                Log.Error("Echec lors de la création de la commande dans la table commandedocument\nErreur: {0}", e);
                return false;
            }
        }

        /// <summary>
        /// Permet de supprimer la ligne correspondante a l'id dans la table commandedocument (Livre)
        /// </summary>
        /// <param name="id"></param>
        public static bool SupprimerCommande(string id)
        {
            try
            {
                string req = "DELETE FROM commandedocument WHERE id = @id";
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@id", id);
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);
                return true;
            }
            catch (Exception e)
            {
                Log.Error("Echec lors de la suppression de la commande dans la table commandedocument\nErreur: {0}", e);
                return false;
            }
        }

        /// <summary>
        /// Permet de supprimer la ligne correspondante a l'id dans la table commande (Universel)
        /// </summary>
        /// <param name="id"></param>
        public static bool SupprimerCommande2(string id)
        {
            try
            {
                string req = "DELETE FROM commande WHERE id = @id";
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@id", id);
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);
                return true;
            }
            catch (Exception e)
            {
                Log.Error("Echec lors de la suppression de la commande dans la table commande\nErreur: {0}", e);
                return false;
            }
        }

        /// <summary>
        /// Permet d'obtenir toutes les infos liées a une commande (Dvd)
        /// </summary>
        /// <returns>La List contenant toutes les infos de chaque commande (Dvd)</returns>
        public static List<CommandeDocumentDvd> GetCommandesDvd()
        {
            List<CommandeDocumentDvd> lesCommandes = null;
            try
            {
                lesCommandes = new List<CommandeDocumentDvd>();
                string req = "SELECT c.id as id_commande, c.dateCommande, c.montant, cd.nbExemplaire, cd.idLivreDvd as idLivre, s.id as id_etat, s.libelle as etat, dd.duree, d.titre, dd.realisateur, dd.synopsis, g.libelle as genre, p.libelle as public, r.libelle as rayon, d.image ";
                req += "FROM `commande` c ";
                req += "LEFT JOIN `commandedocument` cd USING(id) ";
                req += "LEFT JOIN `suivi` s ON s.id = cd.idSuivi ";
                req += "LEFT JOIN `document` d ON d.id = cd.idLivreDvd ";
                req += "LEFT JOIN `genre` g ON d.idGenre = g.id ";
                req += "LEFT JOIN `public` p ON d.idPublic = p.id ";
                req += "LEFT JOIN `rayon` r ON d.idRayon = r.id ";
                req += "LEFT JOIN `dvd` dd ON dd.id = cd.idLivreDvd ";
                req += "WHERE cd.idLivreDvd = dd.id ";
                req += "ORDER BY c.dateCommande DESC";
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqSelect(req, null);

                while (curs.Read())
                {
                    CommandeDocumentDvd commandeDocument = new CommandeDocumentDvd(
                        (string)curs.Field("id_commande"),
                        (DateTime)curs.Field("dateCommande"),
                        (double)curs.Field("montant"),
                        (int)curs.Field("nbExemplaire"),
                        (string)curs.Field("idLivre"),
                        (string)curs.Field("id_etat"),
                        (string)curs.Field("etat"),
                        (int)curs.Field("duree"),
                        (string)curs.Field("titre"),
                        (string)curs.Field("realisateur"),
                        (string)curs.Field("synopsis"),
                        (string)curs.Field("genre"),
                        (string)curs.Field("public"),
                        (string)curs.Field("rayon"),
                        (string)curs.Field("image")
                        );
                    lesCommandes.Add(commandeDocument);
                }
                curs.Close();
                return lesCommandes;
            }
            catch (Exception e)
            {
                Log.Error("Echec lors de la recupération des DVDs depuis la BDD\nErreur: {0}", e);
                return lesCommandes;
            }
        }

        /// <summary>
        /// Permet d'obtenir tout les abonnements dans une List
        /// </summary>
        /// <returns>Une liste comportant tout les abonnenements</returns>
        public static List<AbonnementRevue> GetAbonnementsRevues()
        {
            List<AbonnementRevue> lesCommandes = null;
            try
            {
                lesCommandes = new List<AbonnementRevue>();
                string req = "SELECT c.id, c.dateCommande, a.dateFinAbonnement, a.idRevue, r.empruntable, d.titre, r.periodicite, r.delaiMiseADispo as delai, g.libelle as genre, p.libelle as public, ra.libelle as rayon, d.image, c.montant ";
                req += "FROM commande c ";
                req += "LEFT JOIN abonnement a USING(id) ";
                req += "LEFT JOIN revue r ON a.idRevue = r.id ";
                req += "LEFT JOIN document d ON a.idRevue = d.id ";
                req += "LEFT JOIN genre g ON d.idGenre = g.id ";
                req += "LEFT JOIN public p ON d.idPublic = p.id ";
                req += "LEFT JOIN rayon ra ON d.idRayon = ra.id ";
                req += "WHERE a.idRevue = r.id ";
                req += "ORDER BY c.dateCommande DESC";
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqSelect(req, null);

                while (curs.Read())
                {
                    AbonnementRevue commandeDocument = new AbonnementRevue(
                        (string)curs.Field("id"),
                        (DateTime)curs.Field("dateCommande"),
                        (DateTime)curs.Field("dateFinAbonnement"),
                        (string)curs.Field("idRevue"),
                        (bool)curs.Field("empruntable"),
                        (string)curs.Field("titre"),
                        (string)curs.Field("periodicite"),
                        (int)curs.Field("delai"),
                        (string)curs.Field("genre"),
                        (string)curs.Field("public"),
                        (string)curs.Field("rayon"),
                        (string)curs.Field("image"),
                        (double)curs.Field("montant")
                        );
                    lesCommandes.Add(commandeDocument);
                }
                curs.Close();
                return lesCommandes;
            }
            catch (Exception e)
            {
                Log.Error("Echec lors de la récupération des abonnements de revues depuis la BDD\nErreur: {0}", e);
                return lesCommandes;
            }
        }

        /// <summary>
        /// Creer un abonnement
        /// </summary>
        /// <param name="idCommande"></param>
        /// <param name="dateFinAbonnement"></param>
        /// <param name="revueId"></param>
        public static bool CreerAbonnement(string idCommande, DateTime dateFinAbonnement, string revueId)
        {
            try
            {
                string req = "insert into abonnement values (@idCommande,@dateFinAbonnement,@revueId)";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@idCommande", idCommande},
                    { "@dateFinAbonnement", dateFinAbonnement},
                    { "@revueId", revueId},
                };
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);
                curs.Close();
                return true;
            }
            catch (Exception e)
            {
                Log.Error("Echec lors de la création de l'abonnement dans la table abonnement\nErreur: {0}", e);
                return false;
            }
        }

        /// <summary>
        /// Supprime un abonnement
        /// </summary>
        /// <param name="idCommande"></param>
        public static bool SupprimerAbonnement(string idCommande)
        {
            try
            {
                string req = "DELETE FROM abonnement WHERE id = @id";
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@id", idCommande);
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);
                return true;
            }
            catch (Exception e)
            {
                Log.Error("Echec lors de la suppression de l'abonnement dans la table abonnement\nErreur: {0}", e);
                return false;
            }
        }

        /// <summary>
        /// Permet d'afficher tout les abonnements dont la date de fin est en dessous de 30 jours au login
        /// 
        /// Ne s'execute qu'une fois au démarrage
        /// </summary>
        /// <returns>String comportant toutes les infos a afficher dans une MessageBox</returns>
        public static string GetAbonnementsSub30Days()
        {
            //Fonctionne sans cette partie
            if (nb == 0)
            {
                try
                {
                    string req = "SELECT abonnementsEnDessousTrentreJours() AS string;";
                    BddMySql curs = BddMySql.GetInstance(connectionString);
                    curs.ReqSelect(req, null);
                    string procedure = "";
                    while (curs.Read())
                    {
                        procedure = (string)curs.Field("string");
                        procedure = procedure.Replace(" retourALaLigne ", "\n");
                    }
                    nb++;
                    return procedure;
                }
                catch
                {
                    nb++;
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
           
        /// <summary>
        /// Check si le combo identifiant mdp est valide pour se connecter
        /// 
        /// Si oui return l'id du service
        /// Si non return null;
        /// </summary>
        /// <param name="identifiant"></param>
        /// <param name="mdp"></param>
        public static Service ControleAuthentification(string identifiant, string mdp)
        {
            string req = "SELECT identifiant, service, s.nom FROM utilisateur u ";
            req += "LEFT JOIN service s on s.id = u.service ";
            req += "WHERE u.identifiant = @identifiant AND u.mdp = SHA2(@mdp, 256)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@identifiant", identifiant);
            parameters.Add("@mdp", mdp);
            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, parameters);
            if (curs.Read())
            {
                Service service = new Service((string)curs.Field("identifiant"), (int)curs.Field("service"), (string)curs.Field("nom"));
                Log.Information("L'utilisateur {0} s'est connecté (Appartenant au service {1})", service.Utilisateur, service.Nom);
                curs.Close();
                return service;
            }
            else
            {   
                Log.Error("Echec de la connexion pour le nom d'utilisateur : {0}", identifiant);
                curs.Close();
                return null;
            }
        }
    }
}
