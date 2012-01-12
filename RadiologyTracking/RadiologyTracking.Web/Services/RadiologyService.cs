
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
    using System.Data.Entity;
    using System.Web.Security;

    [EnableClientAccess]
    [RequiresAuthentication]
    public class RadiologyService : DbDomainService<RadiologyContext>
    {
        #region Changes

        public IQueryable<Change> GetChanges()
        {
            return this.DbContext.Changes;
        }

        public IQueryable<Change> GetChangesByDate(DateTime fromDate, DateTime toDate)
        {
            return this.DbContext.Changes.Where(p => p.When <= fromDate && p.When >= toDate);
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

        //public void UpdateChange(Change currentChange)
        //{
        //    this.DbContext.Changes.AttachAsModified(currentChange, this.ChangeSet.GetOriginal(currentChange), this.DbContext);
        //}

        //public void DeleteChange(Change entity)
        //{
        //    DbEntityEntry<Change> entityEntry = this.DbContext.Entry(entity);
        //    if ((entityEntry.State != EntityState.Deleted))
        //    {
        //        entityEntry.State = EntityState.Deleted;
        //    }
        //    else
        //    {
        //        this.DbContext.Changes.Attach(entity);
        //        this.DbContext.Changes.Remove(entity);
        //    }
        //}

        #endregion

        #region Coverages

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
        #endregion

        #region Company

        public IQueryable<Company> GetCompanies()
        {
            return this.DbContext.Companies;
        }
        
        public void UpdateCompany(Company currentCompany)
        {
            this.DbContext.Companies.AttachAsModified(currentCompany, this.ChangeSet.GetOriginal(currentCompany), this.DbContext);
        }

        #endregion


        #region Customers
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
        #endregion

        #region Defects
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
        #endregion

        #region Energies
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
        #endregion

        #region FilmSize
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
        #endregion

        #region Film Trasactions and other Film related functions
        public IQueryable<FilmTransaction> GetFilmTransactions()
        {
            return this.DbContext.FilmTransactions;
        }

        public IQueryable<FilmTransaction> GetFilmTransactionsByFoundryAndDate(String foundryName, DateTime fromDate, DateTime toDate)
        {
            return this.DbContext.FilmTransactions.Where(p =>
                                                            p.Date >= fromDate &&
                                                            p.Date <= toDate &&
                                                            p.Foundry.FoundryName.Contains(foundryName));
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


        /// <summary>
        /// This function returns the data for the stock balance report
        /// </summary>
        /// <param name="foundry"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public IQueryable GetFilmStockReport(Foundry foundry, DateTime fromDate, DateTime toDate)
        {
            var transactions = this.DbContext.FilmTransactions.Where(p => p.Foundry.Equals(foundry))
                .Select(p => new
                {
                    Date = p.Date,
                    SentToHO = p.Direction == Direction.SENT_TO_HO ? p.Area : 0,
                    Consumed = 0,
                    ReceivedFromHO = p.Direction == Direction.SENT_TO_HO ? 0 : p.Area
                });

            var consumptions = this.DbContext.RGReportRows.Where(p => p.RGReport.FixedPattern.Customer.Foundry.Equals(foundry))
                                                            .Select(p => new
                                                            {
                                                                Date = p.RGReport.ReportDate,
                                                                SentToHO = 0,
                                                                Consumed = p.FilmSize.Area,
                                                                ReceivedFromHO = 0
                                                            });

            var all = transactions.Union(consumptions);

            var allByDate = from a in all
                            group a by a.Date into g
                            select new
                            {
                                Date = g.Key,
                                SentToHO = g.Sum(p => p.SentToHO),
                                Consumed = g.Sum(p => p.Consumed),
                                ReceivedFromHO = g.Sum(p => p.ReceivedFromHO),
                            };

            var allByDateInSpan = allByDate.Where(p => p.Date >= fromDate && p.Date <= toDate);

            var openingStockonFromDate = allByDate.Where(p => p.Date < fromDate).Sum(p => p.ReceivedFromHO - p.SentToHO - p.Consumed);

            var stock = from r in allByDateInSpan
                        //opening stock for each date, since openingStockonFromDate is calculated at one shot, remaining should be fast enough
                        let openingStock = openingStockonFromDate + allByDateInSpan
                                                                    .Where(p => p.Date < r.Date)
                                                                    .Sum(p => p.ReceivedFromHO - p.SentToHO - p.Consumed)
                        select new
                        {
                            Date = r.Date,
                            OpeningStock = openingStock,
                            SentToHO = r.SentToHO,
                            Consumed = r.Consumed,
                            ReceivedFromHO = r.ReceivedFromHO,
                            ClosingStock = openingStock + r.ReceivedFromHO - r.SentToHO - r.Consumed
                        };

            return stock;
        }

        public IQueryable GetFilmConsumptionReport(DateTime fromDate, DateTime toDate)
        {
            return from r in this.DbContext.RGReportRows
                   group r by new { r.RGReport, r.Energy, r.FilmSize, r.RowType } into g
                   select new
                   {
                       Report = g.Key.RGReport,
                       Energy = g.Key.Energy,
                       FilmSize = g.Key.FilmSize,
                       RowType = g.Key.RowType,
                       Area = g.Sum(p => p.FilmSize.Area)
                   };
        }

        #endregion

        #region Fixed Patterns
        
        public IQueryable<FixedPattern> GetFixedPatterns(String filter = "")
        {
            return this.DbContext.FixedPatterns.Where(p =>
                                                        p.FPNo.Contains(filter) ||
                                                        p.Description.Contains(filter));
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
        

        public IQueryable GetFixedPatternPerformanceReport(String fpNo)
        {
            return from r in this.DbContext.RGReportRows
                   where r.RGReport.FixedPattern.FPNo == fpNo
                   group r by new { r.RGReport.RTNo, r.RGReport.ReportDate } into g
                   select new
                   {
                       RTNo = g.Key.RTNo,
                       Date = g.Key.ReportDate,
                       Locations = from rep in g
                                   group rep by new { rep.Location, rep.Segment, rep.Observations } into repg
                                   select new
                                   {
                                       Location = repg.Key.Location,
                                       Segments = from row in repg
                                                  group repg by new { repg.Key.Segment, repg.Key.Observations } into segg
                                                  select new
                                                  {
                                                      Segment = segg.Key,
                                                      Observations = String.Join(",", segg.Key.Observations
                                                                                        .Select(p => p.ToString())
                                                                                        .ToList())
                                                  }
                                   }
                   };
        }

        #endregion

        #region FixedPattern Templates
        public IQueryable<FixedPatternTemplate> GetFixedPatternTemplates()
        {
            return this.DbContext.FixedPatternTemplates;
        }

        public IQueryable<FixedPatternTemplate> GetFixedPatternTemplatesForFP(String fixedPatternNo, String coverage)
        {
            return this.DbContext.FixedPatternTemplates.Where(p =>
                                                                p.FixedPattern.FPNo.Equals(fixedPatternNo) &&
                                                                p.Coverage.CoverageName.Equals(coverage));
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
        #endregion

        #region Foundries
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
        #endregion

        #region FP Template Rows
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
        #endregion

        #region Observations
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
        #endregion

        #region RG Reports
        public IQueryable<RGReport> GetRGReports(DateTime fromDate, DateTime toDate)
        {
            return this.DbContext.RGReports.Where(p =>
                                                    p.ReportDate <= fromDate &&
                                                    p.ReportDate >= toDate);
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
        #endregion

        #region RG Report Rows
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
        #endregion

        #region Technicians
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

        public IQueryable GetShiftWisePerformanceReport(DateTime fromDate, DateTime toDate, Technician technician = null)
        {
            return from r in this.DbContext.RGReportRows
                   where r.Technician == (technician == null ? r.Technician : technician)
                   && r.RGReport.ReportDate >= fromDate && r.RGReport.ReportDate <= toDate
                   group r by new { r.RGReport.ReportDate, r.RGReport.Shift } into g
                   let total = g.Count()
                   let retakes = g.Where(p => p.Remark == Remark.RETAKE).Count()
                   let totalArea = g.Sum(p => p.FilmSize.Area)
                   let retakeArea = g.Where(p => p.Remark == Remark.RETAKE).Sum(p => p.FilmSize.Area)
                   select new
                   {
                       Technicians = String.Join(",", g.Select(p => p.Technician.Name).ToList()),
                       Date = g.Key.ReportDate,
                       Shift = g.Key.Shift,
                       FilmArea = from f in g
                                  group f by f.FilmSize into fg
                                  select new
                                  {
                                      FilmSize = fg.Key,
                                      Total = fg.Count(),
                                      RT = fg.Where(p => p.Remark == Remark.RETAKE).Count()
                                  },
                       TotalFilmsTaken = total,
                       TotalRetakes = retakes,
                       RTPercent = (retakes * 100 / total),
                       RTPercentByArea = (retakeArea * 100 / totalArea)
                   };
        }
        #endregion

        #region Thickness Ranges
        public IQueryable<ThicknessRangeForEnergy> GetThicknessRangesForEnergy(String filter = "")
        {
            return this.DbContext.ThicknessRangesForEnergy.Where(p => p.Energy.Name.Contains(filter));
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
        #endregion
        
        #region Users

        //public IQueryable GetUsers()
        //{
        //    return Membership.GetAllUsers().AsQueryable();
        //}

        //public void InsertUser(User user)
        //{
        //    using (UserRegistrationService urs = new UserRegistrationService())
        //    {
        //       // urs.CreateUser(user, user.Password);
        //    }
            
        //}

        //public void UpdateUser(User user)
        //{
        //    //this.DbContext.Users.AttachAsModified(currentUser, this.ChangeSet.GetOriginal(currentUser), this.DbContext);
        //    //Membership.UpdateUser((MembershipUser)user);
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
        //        //this.DbContext.Users.Attach(entity);
        //        //this.DbContext.Users.Remove(entity);
        //    }
        //}


        #endregion

        #region Welders
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
        #endregion
    }
}