namespace Mediatek86.metier
{
    public class Service
    {
        private readonly string utilisateur;
        private readonly int service;
        private readonly string nom;

        public Service(string utilisateur, int service, string nom)
        {
            this.utilisateur = utilisateur;
            this.service = service;
            this.nom = nom;
        }

        public string Utilisateur { get => utilisateur; }
        public int ServiceInt { get => service; }
        public string Nom { get => nom; }
    }
}
