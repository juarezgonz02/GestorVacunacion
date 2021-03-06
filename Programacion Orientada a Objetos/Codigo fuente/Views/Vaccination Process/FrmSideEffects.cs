using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using VaccinationManagement.Context;
using VaccinationManagement.Models;


namespace VaccinationManagement.Views
{
    public partial class FrnSideEffects : Form
    {
        private List<SideEffect> sideEffects;
        private Appointment selectedAppointment; 
        
        public FrnSideEffects(Appointment selectedAppointment)
        {
            this.selectedAppointment = selectedAppointment;
            sideEffects = new List<SideEffect>();
            
            InitializeComponent();
        }

        private void FrnSideEffects_Load(object sender, EventArgs e)
        {
            var db = new VaccinationContext();
            
            var sideEffectsTypes = db.SideEffectTypes.ToList();
            cmbEffectTypess.DataSource = sideEffectsTypes;
            cmbEffectTypess.DisplayMember = "Effect";
        }
      
        private void btnFinish_Click(object sender, EventArgs e)
        {
            try
            {
                var db = new VaccinationContext();
                sideEffects.ForEach(effect =>
                {
                    db.SideEffects.Add(effect);
                });

                db.SaveChanges();
                
                using (var register = new FrmRegister())
                {
                    var result = register.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        
                    }
                    
                    
                }

                Close();


            }
            catch (DbUpdateException exception)
            {
                MessageBox.Show($"{exception}");
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (selectedAppointment.VaccineDate != null)
            {
                var vaccinationDate = (DateTime) selectedAppointment.VaccineDate;

                try
                {
                    var sideEffect = new SideEffect()
                    {
                        IdAppointment = selectedAppointment.Id,
                        EffectTime = vaccinationDate.AddMinutes(
                            Int32.Parse(txtMinutes.Text)),
                        IdEffect = ((SideEffectType) cmbEffectTypess.SelectedItem).Id
                    };

                    sideEffects.Add(sideEffect);
                    
                    using (var add = new FrmDatesEffectsUpdate())
                    {
                        var result = add.ShowDialog();
                        if (result == DialogResult.OK) ;


                    }

                   
                }
                catch (FormatException exception)
                {
                    using (var invalid = new FrmInvalidData())
                    {
                        var result = invalid.ShowDialog();
                        if (result == DialogResult.OK) ;
                                                    
                    }
                }
                finally
                {
                    txtMinutes.Text = "";
                }
            }
            else
            {
                using (var RegisterVaccination = new FrmRegisterVaccination())
                {
                    var result = RegisterVaccination.ShowDialog();
                    if (result == DialogResult.OK) ;
                                                    
                }
            }
        }

        
        
        
    }
}