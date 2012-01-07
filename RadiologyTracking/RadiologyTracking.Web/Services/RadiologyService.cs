
namespace RadiologyTracking.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using RadiologyTracking.Web.Models;
    using RadiologyTracking.Web;
    using System.Data.Entity.Infrastructure;
    using System.Data;
    using System.ServiceModel.DomainServices.EntityFramework;

    [EnableClientAccess]
    public class RadiologyService : DbDomainService<RadiologyContext>
    {
        public IQueryable<Change> GetChanges()
        {
            return this.DbContext.Changes;
        }

        public void InsertChange(Change entity)
        {
            DbEntityEntry<Change> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.Changes.Add(entity);
            }
        }

        public void UpdateChange(Change currentChange)
        {
            this.DbContext.Changes.AttachAsModified(currentChange, this.ChangeSet.GetOriginal(currentChange), this.DbContext);
        }

        public void DeleteChange(Change entity)
        {
            DbEntityEntry<Change> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.Changes.Attach(entity);
                this.DbContext.Changes.Remove(entity);
            }
        }

        public IQueryable<Coverage> GetCoverages()
        {
            return this.DbContext.Coverages;
        }

        public void InsertCoverage(Coverage entity)
        {
            DbEntityEntry<Coverage> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.Coverages.Add(entity);
            }
        }

        public void UpdateCoverage(Coverage currentCoverage)
        {
            this.DbContext.Coverages.AttachAsModified(currentCoverage, this.ChangeSet.GetOriginal(currentCoverage), this.DbContext);
        }

        public void DeleteCoverage(Coverage entity)
        {
            DbEntityEntry<Coverage> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.Coverages.Attach(entity);
                this.DbContext.Coverages.Remove(entity);
            }
        }


        public IQueryable<Customer> GetCustomers()
        {
            return this.DbContext.Customers;
        }

        public void InsertCustomer(Customer entity)
        {
            DbEntityEntry<Customer> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.Customers.Add(entity);
            }
        }

        public void UpdateCustomer(Customer currentCustomer)
        {
            this.DbContext.Customers.AttachAsModified(currentCustomer, this.ChangeSet.GetOriginal(currentCustomer), this.DbContext);
        }

        public void DeleteCustomer(Customer entity)
        {
            DbEntityEntry<Customer> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.Customers.Attach(entity);
                this.DbContext.Customers.Remove(entity);
            }
        }


        public IQueryable<Defect> GetDefects()
        {
            return this.DbContext.Defects;
        }

        public void InsertDefect(Defect entity)
        {
            DbEntityEntry<Defect> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.Defects.Add(entity);
            }
        }

        public void UpdateDefect(Defect currentDefect)
        {
            this.DbContext.Defects.AttachAsModified(currentDefect, this.ChangeSet.GetOriginal(currentDefect), this.DbContext);
        }

        public void DeleteDefect(Defect entity)
        {
            DbEntityEntry<Defect> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.Defects.Attach(entity);
                this.DbContext.Defects.Remove(entity);
            }
        }


        public IQueryable<Energy> GetEnergies()
        {
            return this.DbContext.Energies;
        }

        public void InsertEnergy(Energy entity)
        {
            DbEntityEntry<Energy> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.Energies.Add(entity);
            }
        }

        public void UpdateEnergy(Energy currentEnergy)
        {
            this.DbContext.Energies.AttachAsModified(currentEnergy, this.ChangeSet.GetOriginal(currentEnergy), this.DbContext);
        }

        public void DeleteEnergy(Energy entity)
        {
            DbEntityEntry<Energy> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.Energies.Attach(entity);
                this.DbContext.Energies.Remove(entity);
            }
        }


        public IQueryable<FilmSize> GetFilmSizes()
        {
            return this.DbContext.FilmSizes;
        }

        public void InsertFilmSize(FilmSize entity)
        {
            DbEntityEntry<FilmSize> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.FilmSizes.Add(entity);
            }
        }

        public void UpdateFilmSize(FilmSize currentFilmSize)
        {
            this.DbContext.FilmSizes.AttachAsModified(currentFilmSize, this.ChangeSet.GetOriginal(currentFilmSize), this.DbContext);
        }

        public void DeleteFilmSize(FilmSize entity)
        {
            DbEntityEntry<FilmSize> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.FilmSizes.Attach(entity);
                this.DbContext.FilmSizes.Remove(entity);
            }
        }


        public IQueryable<FilmTransaction> GetFilmTransactions()
        {
            return this.DbContext.FilmTransactions;
        }

        public void InsertFilmTransaction(FilmTransaction entity)
        {
            DbEntityEntry<FilmTransaction> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.FilmTransactions.Add(entity);
            }
        }

        public void UpdateFilmTransaction(FilmTransaction currentFilmTransaction)
        {
            this.DbContext.FilmTransactions.AttachAsModified(currentFilmTransaction, this.ChangeSet.GetOriginal(currentFilmTransaction), this.DbContext);
        }

        public void DeleteFilmTransaction(FilmTransaction entity)
        {
            DbEntityEntry<FilmTransaction> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.FilmTransactions.Attach(entity);
                this.DbContext.FilmTransactions.Remove(entity);
            }
        }


        public IQueryable<FixedPattern> GetFixedPatterns()
        {
            return this.DbContext.FixedPatterns;
        }

        public void InsertFixedPattern(FixedPattern entity)
        {
            DbEntityEntry<FixedPattern> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.FixedPatterns.Add(entity);
            }
        }

        public void UpdateFixedPattern(FixedPattern currentFixedPattern)
        {
            this.DbContext.FixedPatterns.AttachAsModified(currentFixedPattern, this.ChangeSet.GetOriginal(currentFixedPattern), this.DbContext);
        }

        public void DeleteFixedPattern(FixedPattern entity)
        {
            DbEntityEntry<FixedPattern> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.FixedPatterns.Attach(entity);
                this.DbContext.FixedPatterns.Remove(entity);
            }
        }


        public IQueryable<FixedPatternTemplate> GetFixedPatternTemplates()
        {
            return this.DbContext.FixedPatternTemplates;
        }

        public void InsertFixedPatternTemplate(FixedPatternTemplate entity)
        {
            DbEntityEntry<FixedPatternTemplate> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.FixedPatternTemplates.Add(entity);
            }
        }

        public void UpdateFixedPatternTemplate(FixedPatternTemplate currentFixedPatternTemplate)
        {
            this.DbContext.FixedPatternTemplates.AttachAsModified(currentFixedPatternTemplate, this.ChangeSet.GetOriginal(currentFixedPatternTemplate), this.DbContext);
        }

        public void DeleteFixedPatternTemplate(FixedPatternTemplate entity)
        {
            DbEntityEntry<FixedPatternTemplate> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.FixedPatternTemplates.Attach(entity);
                this.DbContext.FixedPatternTemplates.Remove(entity);
            }
        }


        public IQueryable<Foundry> GetFoundries()
        {
            return this.DbContext.Foundries;
        }

        public void InsertFoundry(Foundry entity)
        {
            DbEntityEntry<Foundry> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.Foundries.Add(entity);
            }
        }

        public void UpdateFoundry(Foundry currentFoundry)
        {
            this.DbContext.Foundries.AttachAsModified(currentFoundry, this.ChangeSet.GetOriginal(currentFoundry), this.DbContext);
        }

        public void DeleteFoundry(Foundry entity)
        {
            DbEntityEntry<Foundry> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.Foundries.Attach(entity);
                this.DbContext.Foundries.Remove(entity);
            }
        }


        public IQueryable<FPTemplateRow> GetFPTemplateRows()
        {
            return this.DbContext.FPTemplateRows;
        }

        public void InsertFPTemplateRow(FPTemplateRow entity)
        {
            DbEntityEntry<FPTemplateRow> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.FPTemplateRows.Add(entity);
            }
        }

        public void UpdateFPTemplateRow(FPTemplateRow currentFPTemplateRow)
        {
            this.DbContext.FPTemplateRows.AttachAsModified(currentFPTemplateRow, this.ChangeSet.GetOriginal(currentFPTemplateRow), this.DbContext);
        }

        public void DeleteFPTemplateRow(FPTemplateRow entity)
        {
            DbEntityEntry<FPTemplateRow> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.FPTemplateRows.Attach(entity);
                this.DbContext.FPTemplateRows.Remove(entity);
            }
        }


        public IQueryable<Observation> GetObservations()
        {
            return this.DbContext.Observations;
        }

        public void InsertObservation(Observation entity)
        {
            DbEntityEntry<Observation> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.Observations.Add(entity);
            }
        }

        public void UpdateObservation(Observation currentObservation)
        {
            this.DbContext.Observations.AttachAsModified(currentObservation, this.ChangeSet.GetOriginal(currentObservation), this.DbContext);
        }

        public void DeleteObservation(Observation entity)
        {
            DbEntityEntry<Observation> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.Observations.Attach(entity);
                this.DbContext.Observations.Remove(entity);
            }
        }


        public IQueryable<RGReport> GetRGReports()
        {
            return this.DbContext.RGReports;
        }

        public void InsertRGReport(RGReport entity)
        {
            DbEntityEntry<RGReport> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.RGReports.Add(entity);
            }
        }

        public void UpdateRGReport(RGReport currentRGReport)
        {
            this.DbContext.RGReports.AttachAsModified(currentRGReport, this.ChangeSet.GetOriginal(currentRGReport), this.DbContext);
        }

        public void DeleteRGReport(RGReport entity)
        {
            DbEntityEntry<RGReport> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.RGReports.Attach(entity);
                this.DbContext.RGReports.Remove(entity);
            }
        }


        public IQueryable<RGReportRow> GetRGReportRows()
        {
            return this.DbContext.RGReportRows;
        }

        public void InsertRGReportRow(RGReportRow entity)
        {
            DbEntityEntry<RGReportRow> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.RGReportRows.Add(entity);
            }
        }

        public void UpdateRGReportRow(RGReportRow currentRGReportRow)
        {
            this.DbContext.RGReportRows.AttachAsModified(currentRGReportRow, this.ChangeSet.GetOriginal(currentRGReportRow), this.DbContext);
        }

        public void DeleteRGReportRow(RGReportRow entity)
        {
            DbEntityEntry<RGReportRow> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.RGReportRows.Attach(entity);
                this.DbContext.RGReportRows.Remove(entity);
            }
        }


        public IQueryable<Technician> GetTechnicians()
        {
            return this.DbContext.Technicians;
        }

        public void InsertTechnician(Technician entity)
        {
            DbEntityEntry<Technician> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.Technicians.Add(entity);
            }
        }

        public void UpdateTechnician(Technician currentTechnician)
        {
            this.DbContext.Technicians.AttachAsModified(currentTechnician, this.ChangeSet.GetOriginal(currentTechnician), this.DbContext);
        }

        public void DeleteTechnician(Technician entity)
        {
            DbEntityEntry<Technician> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.Technicians.Attach(entity);
                this.DbContext.Technicians.Remove(entity);
            }
        }


        public IQueryable<ThicknessRangeForEnergy> GetThicknessRangesForEnergy()
        {
            return this.DbContext.ThicknessRangesForEnergy;
        }

        public void InsertThicknessRangeForEnergy(ThicknessRangeForEnergy entity)
        {
            DbEntityEntry<ThicknessRangeForEnergy> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.ThicknessRangesForEnergy.Add(entity);
            }
        }

        public void UpdateThicknessRangeForEnergy(ThicknessRangeForEnergy currentThicknessRangeForEnergy)
        {
            this.DbContext.ThicknessRangesForEnergy.AttachAsModified(currentThicknessRangeForEnergy, this.ChangeSet.GetOriginal(currentThicknessRangeForEnergy), this.DbContext);
        }

        public void DeleteThicknessRangeForEnergy(ThicknessRangeForEnergy entity)
        {
            DbEntityEntry<ThicknessRangeForEnergy> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.ThicknessRangesForEnergy.Attach(entity);
                this.DbContext.ThicknessRangesForEnergy.Remove(entity);
            }
        }

        public IQueryable<Welder> GetWelders()
        {
            return this.DbContext.Welders;
        }

        public void InsertWelder(Welder entity)
        {
            DbEntityEntry<Welder> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.Welders.Add(entity);
            }
        }

        public void UpdateWelder(Welder currentWelder)
        {
            this.DbContext.Welders.AttachAsModified(currentWelder, this.ChangeSet.GetOriginal(currentWelder), this.DbContext);
        }

        public void DeleteWelder(Welder entity)
        {
            DbEntityEntry<Welder> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.Welders.Attach(entity);
                this.DbContext.Welders.Remove(entity);
            }
        }

        //public IQueryable<User> GetUsers()
        //{
        //    return this.DbContext.Users;
        //}

        //public void InsertUser(User entity)
        //{
        //    DbEntityEntry<User> entityEntry = this.DbContext.Entry(entity);
        //    if ((entityEntry.State != EntityState.Detached))
        //    {
        //        entityEntry.State = EntityState.Added;
        //    }
        //    else
        //    {
        //        this.DbContext.Users.Add(entity);
        //    }
        //}

        //public void UpdateUser(User currentUser)
        //{
        //    this.DbContext.Users.AttachAsModified(currentUser, this.ChangeSet.GetOriginal(currentUser), this.DbContext);
        //}

        //public void DeleteUser(User entity)
        //{
        //    DbEntityEntry<User> entityEntry = this.DbContext.Entry(entity);
        //    if ((entityEntry.State != EntityState.Deleted))
        //    {
        //        entityEntry.State = EntityState.Deleted;
        //    }
        //    else
        //    {
        //        this.DbContext.Users.Attach(entity);
        //        this.DbContext.Users.Remove(entity);
        //    }
        //}

    }
}