using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mediatek86.controleur;
using System;
using Mediatek86.metier;


namespace Mediatek86.vue.Tests
{
    [TestClass()]
    public class FrmMediatekTests
    {
        /// <summary>
        /// Test unitaire sur la fonction ParutionDansAbonnement du formulaire FrmMediatek
        /// </summary>
        [TestMethod()]
        public void ParutionDansAbonnementTest()
        {
            DateTime dateAujourdhui = DateTime.Today;
            DateTime dateDans30Jours = dateAujourdhui.AddDays(30);
            DateTime dateDans2Jours = dateAujourdhui.AddDays(2);
            DateTime dateDans31Jours = dateAujourdhui.AddDays(31);
            FrmMediatek frmMediatek = new FrmMediatek(new Controle(), new Service("admin", 1, "Administratif"));
            Assert.AreEqual(true, frmMediatek.ParutionDansAbonnement(dateAujourdhui, dateDans30Jours, dateDans2Jours), "devrait reussir");
            Assert.AreEqual(false, frmMediatek.ParutionDansAbonnement(dateAujourdhui, dateDans30Jours, dateDans31Jours), "devrait échouer: Date31Jours n'est pas comprise entre aujoud'hui et dans 30 jours");
        }
    }
}
