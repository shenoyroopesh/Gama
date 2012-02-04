
namespace RadiographyTracking.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using RadiographyTracking.Web.Models;
    using RadiographyTracking.Web;
    using System.Data.Entity.Infrastructure;
    using System.Data;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.Data.Entity;
    using System.Web.Security;
    using System.Collections;
    using System.Data.Objects;
    using RadiographyTracking.Web.Utility;

    [EnableClientAccess()]
    [RequiresAuthentication()]
    public class RadiographyService : DbDomainService<RadiographyContext>
    {
        #region Changes

        public IQueryable<Change> GetChanges()
        {
            return this.DbContext.Changes;
        }

        public IQueryable<Change> GetChangesByDate(DateTime fromDate, DateTime toDate)
        {
            fromDate = fromDate.Date;
            toDate = toDate.Date.AddDays(1);
            return this.DbContext.Changes.Where(p => p.When >= fromDate && p.When <= toDate);
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

        /// <summary>
        /// Gets the customers filtered by a filter query
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IQueryable<Customer> GetCustomersFiltered(String filter)
        {
            return this.DbContext.Customers.Where(p => p.CustomerName.Contains(filter) ||
                                                     p.Address.Contains(filter) ||
                                                     p.Email.Contains(filter) ||
                                                     p.Foundry.FoundryName.Contains(filter));
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

        public IQueryable<Direction> GetDirections()
        {
            return this.DbContext.Directions;
        }

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

        public IQueryable<FilmTransaction> GetFilmTransactionsByDate(DateTime fromDate, DateTime toDate)
        {
            //to ensure that time component of the date does not make some dates get excluded
            fromDate = fromDate.Date;
            toDate = toDate.Date.AddDays(1);
            return this.DbContext.FilmTransactions.Where(p =>
                                                            p.Date >= fromDate &&
                                                                p.Date <= toDate).OrderBy(p => p.Date);
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
        public IEnumerable<FilmStockReportRow> GetFilmStockReport(int foundryId, DateTime fromDate, DateTime toDate)
        {
            var sentToHO = this.DbContext.Directions.Single(p => p.Value == "SENT_TO_HO");

            var transactions = this.DbContext.FilmTransactions.Where(p => p.FoundryID.Equals(foundryId))
                .Select(p => new
                {
                    Date = p.Date,
                    SentToHO = p.Direction.ID == sentToHO.ID ? p.Area : 0,
                    Consumed = 0,
                    ReceivedFromHO = p.Direction.ID == sentToHO.ID ? 0 : p.Area
                });

            var consumptions = this.DbContext.RGReportRows.Where(p => p.RGReport.FixedPattern.Customer.FoundryID.Equals(foundryId))
                                                            .Select(p => new
                                                            {
                                                                Date = p.RGReport.ReportDate,
                                                                SentToHO = 0,
                                                                Consumed = p.FilmSize.Area,
                                                                ReceivedFromHO = 0
                                                            });

            var all = transactions.Union(consumptions).ToList();

            var allByDate = from a in all
                            group a by a.Date.Date into g
                            select new
                            {
                                Date = g.Key,
                                SentToHO = g.Sum(p => p.SentToHO),
                                Consumed = g.Sum(p => p.Consumed),
                                ReceivedFromHO = g.Sum(p => p.ReceivedFromHO),
                            };

            var allByDateInSpan = allByDate.Where(p => p.Date >= fromDate && p.Date <= toDate);

            var openingStockonFromDate = allByDate.Where(p => p.Date < fromDate).Sum(p => p.ReceivedFromHO - p.SentToHO - p.Consumed);

            var stock = (from r in allByDateInSpan
                         //opening stock for each date, since openingStockonFromDate is calculated at one shot, remaining should be fast enough
                         let openingStock = openingStockonFromDate + allByDateInSpan
                                                                     .Where(p => p.Date < r.Date)
                                                                     .Sum(p => p.ReceivedFromHO - p.SentToHO - p.Consumed)
                         select new FilmStockReportRow
                         {
                             ID = Guid.NewGuid(), //just to ensure rows are unique
                             Date = r.Date,
                             OpeningStock = openingStock,
                             SentToHO = r.SentToHO,
                             Consumed = r.Consumed,
                             ReceivedFromHO = r.ReceivedFromHO,
                             ClosingStock = openingStock + r.ReceivedFromHO - r.SentToHO - r.Consumed
                         }).OrderBy(p => p.Date);

            return stock;
        }

        public IEnumerable<FilmConsumptionReportRow> GetFilmConsumptionReport(int foundryId, DateTime fromDate, DateTime toDate)
        {

            var intermediate = (from r in this.DbContext.RGReportRows
                                orderby r.RGReport.ReportDate, r.RGReport.ReportNo
                                let rowFID = r.RGReport.FixedPattern.Customer.FoundryID
                                where rowFID == (foundryId == -1 ? rowFID : foundryId)
                                && r.RowType.Value != "RETAKE"               //retakes are not considered in the film consumption report
                                group r by new { r.RGReport, r.RGReport.FixedPattern, r.Energy, r.RowType } into g
                                select new { Key = g.Key, Area = g.Sum(p => p.FilmSize.Area) }).ToList();


            return from g in intermediate
                   select new FilmConsumptionReportRow
                   {
                       ID = Guid.NewGuid(),
                       ReportNo = g.Key.RGReport.ReportNo,
                       Date = g.Key.RGReport.ReportDate.ToString("dd-MM-yyyy"),
                       FPNo = g.Key.RGReport.FixedPattern.FPNo,
                       RTNo = g.Key.RGReport.RTNo,
                       Energy = g.Key.Energy.Name,
                       RowType = g.Key.RowType.Value,
                       Area = g.Area
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
            //check whether FP no exists, if it does throw an exception
            var existingFP = this.DbContext.FixedPatterns.Where(p => p.FPNo == entity.FPNo).FirstOrDefault();

            if (existingFP != null) throw new ArgumentException("Another FP with this FP No " + entity.FPNo + " Already exists");

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


        public IEnumerable<FixedPatternPerformanceRow> GetFixedPatternPerformanceReport(string fpNo)
        {
            var ctx = this.DbContext;

            //fetch required rows from database first, then form the complex object - linq to entities doesn't support
            //creating complex objects directly
            var rows = (from r in ctx.RGReportRows
                        where r.RGReport != null &&
                        r.RGReport.FixedPattern.FPNo == fpNo &&
                            //ensure that the report has at least one defect
                        r.RGReport.RGReportRows.Where(p => (p.Observations ?? "").Trim() != "NSD").Count() > 0
                        select new
                        {
                            RTNo = r.RGReport.RTNo,
                            ReportDate = r.RGReport.ReportDate,
                            Location = r.Location,
                            Segment = r.Segment,
                            Observations = r.Observations
                        }).ToList();


            var report = (from r in rows
                         orderby r.RTNo, r.ReportDate
                         group r by new { r.RTNo, r.ReportDate } into g
                         select new FixedPatternPerformanceRow
                         {
                             ID = Guid.NewGuid(),
                             RTNo = g.Key.RTNo,
                             Date = g.Key.ReportDate,
                             Locations = (from rep in g
                                         group rep by new { rep.Location } into repg
                                         select new LocationClass
                                         {
                                             ID = Guid.NewGuid(),
                                             Location = repg.Key.Location,
                                             Segments = (from row in repg.Select(p => new { p.Segment, p.Observations }).ToArray()
                                                        group row by new { row.Segment, row.Observations } into segg
                                                        select new SegmentClass
                                                        {
                                                            ID = Guid.NewGuid(),
                                                            Segment = segg.Key.Segment,
                                                            Observations = segg.Key.Observations
                                                        }).ToList()
                                         }).ToList()
                         }).ToList();

            #region update the guids for childs

            foreach(var row in report)
            {
                foreach (var loc in row.Locations)
                {
                    loc.FixedPatternPerformanceRowID = row.ID;
                    foreach (var seg in loc.Segments)
                    {
                        seg.LocationID = loc.ID;
                    }
                }
            }

            #endregion

            return report;
        }

        #endregion

        #region FixedPattern Templates
        public IQueryable<FixedPatternTemplate> GetFixedPatternTemplates()
        {
            return this.DbContext.FixedPatternTemplates;
        }

        /// <summary>
        /// Returns the Fixed Pattern template corresponding to this fP no and coverage. If none exists, returns a new 
        /// template
        /// </summary>
        /// <param name="fixedPatternNo"></param>
        /// <param name="strCoverage"></param>
        /// <returns></returns>
        public FixedPatternTemplate GetFixedPatternTemplateForFP(String fixedPatternNo, String strCoverage)
        {
            FixedPattern fixedPattern;
            Coverage coverage;

            try
            {
                fixedPattern = this.DbContext.FixedPatterns.Single(p => p.FPNo == fixedPatternNo);
                coverage = this.DbContext.Coverages.Single(p => p.CoverageName == strCoverage);
            }
            catch (InvalidOperationException e)
            {
                return null;
            }

            FixedPatternTemplate fpTemplate = this.DbContext.FixedPatternTemplates.Include(p => p.FPTemplateRows.Select(r => r.FilmSize))
                                                                                  .Include(p=>p.FixedPattern.Customer.Foundry)
                                                                                 .Where(p =>
                                                                                        p.FixedPattern.FPNo == fixedPattern.FPNo &&
                                                                                        p.Coverage.CoverageName == coverage.CoverageName).FirstOrDefault();

            if (fpTemplate != null)
            {
                return fpTemplate;
            }
            else
            {
                //create a new fixed pattern template for this combination
                fpTemplate = new FixedPatternTemplate() { FixedPattern = fixedPattern, Coverage = coverage };
                DbContext.FixedPatternTemplates.Add(fpTemplate);
                //save the fixed pattern template before sending it back, so that it has an ID
                DbContext.SaveChanges();
                return fpTemplate;
            }
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
            //filter if the current user is restricted to a single foundry
            MembershipUser mUser = Membership.GetUser();
            var role = Roles.GetRolesForUser(mUser.UserName).First();
            User user = mUser.GetUser();

            if ((new String[] { "admin", "managing director" }).Contains(role.ToLower()))
                return this.DbContext.Foundries;
            else
                return this.DbContext.Foundries.Where(p => p.FoundryName == user.Foundry);
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
            //required to avoid exception when multiple fptemplaterows are updated together. 
            //The FilmSizeID will take care of the association
            currentFPTemplateRow.FilmSize = null;

            //check if fixedpatternid is zero, if so this row should be deleted not updated
            if (currentFPTemplateRow.FixedPatternTemplateID == 0)
            {
                DeleteFPTemplateRow(currentFPTemplateRow);
                return;
            }

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

        #region Remarks

        public IQueryable<Remark> GetRemarks()
        {
            return this.DbContext.Remarks;
        }

        #endregion

        #region RG Reports

        public IQueryable<RGReport> GetRGReports(String RGReportNo)
        {
            return this.DbContext.RGReports.Include(p => p.RGReportRows.Select(r => r.Remark))
                                            .Where(p => p.ReportNo == RGReportNo);
        }

        public IQueryable<RGReport> GetRGReportsByDate(DateTime fromDate, DateTime toDate)
        {
            //to ensure that time component of the date does not make some dates get excluded
            fromDate = fromDate.Date;
            toDate = toDate.Date.AddDays(1);
            return this.DbContext.RGReports.Where(p =>
                                                    p.ReportDate >= fromDate &&
                                                    p.ReportDate <= toDate);
        }


        public IQueryable<RGReport> GetRGReportsByFPNo(String fpNo)
        {
            return this.DbContext.RGReports.Include(p => p.FixedPattern)
                                            .Where(p => p.FixedPattern.FPNo == fpNo);
        }

        public RGReport GetNewRGReport(String strFPNo, String strCoverage, String rtNo)
        {
            //check if there is an existing report with this combination
            FixedPattern fp = DbContext.FixedPatterns.FirstOrDefault(p => p.FPNo == strFPNo);
            Coverage coverage = DbContext.Coverages.FirstOrDefault(p => p.CoverageName == strCoverage);

            if (fp == null) return null;
            if (coverage == null) return null;

            int fpID = fp.ID;
            int coverageID = coverage.ID;
            FixedPatternTemplate fpTemplate = this.GetFixedPatternTemplateForFP(strFPNo, strCoverage);
            
            //get the latest report in the sequence
            //can't use Last() here, have to use first() since this gets converted into a store query
            var rgReport = DbContext.RGReports.Include(r => r.RGReportRows.Select(p => p.Remark)).Include(p => p.Status)
                                                            .Where(p => p.FixedPatternID == fpID &&
                                                              p.CoverageID == coverageID &&
                                                              p.RTNo == rtNo)
                                                            .OrderByDescending(p => p.ID).FirstOrDefault();

            String nextReportNumber = fpTemplate.FixedPattern.Customer.Foundry.getNextReportNumber(DbContext);

            if (rgReport == null)
            {
                //create new report with existing fptemplate
                rgReport = new RGReport(fpTemplate, rtNo, nextReportNumber, DbContext);
                this.DbContext.RGReports.Add(rgReport);
                this.DbContext.SaveChanges();
                return rgReport;
            }
            else
            {
                //if this RT no is already complete, then no new report to be created
                if (rgReport.Status != null && rgReport.Status.Status == "COMPLETE") return null;

                //if any remark for the earlier report is pending, return it again
                if (rgReport.RGReportRows.Where(p => p.Remark == null).Count() > 0) 
                    return rgReport;

                //else create a new child report
                rgReport = new RGReport(rgReport, nextReportNumber, DbContext);
                this.DbContext.RGReports.Add(rgReport);
                this.DbContext.SaveChanges();
                return rgReport;
            }
        }


        public IEnumerable<RTStatusReportRow> GetRTStatus(int foundryId, DateTime fromDate, DateTime toDate)
        {
            //from date and to date to not consider time
            fromDate = fromDate.Date;
            toDate = toDate.Date.AddDays(1);

            var intermediate = (from r in this.DbContext.RGReports
                                let rowFId = r.FixedPattern.Customer.FoundryID
                                where rowFId == (foundryId == -1 ? rowFId : foundryId)
                                && r.ReportDate >= fromDate && r.ReportDate < toDate
                                group r by new { r.FixedPattern, r.RTNo } into g
                                select new
                                {
                                    ID = Guid.NewGuid(),
                                    FPNo = g.Key.FixedPattern.FPNo,
                                    RTNo = g.Key.RTNo,
                                    Date = g.OrderByDescending(p => p.ReportDate).FirstOrDefault().ReportDate,
                                    Repairs = g.OrderByDescending(p => p.ReportDate).FirstOrDefault().RGReportRows.Where(p => p.Remark != null).Where(p => p.Remark.Value == "REPAIR").Count(),
                                    Retakes = g.OrderByDescending(p => p.ReportDate).FirstOrDefault().RGReportRows.Where(p => p.Remark != null).Where(p => p.Remark.Value == "RETAKE").Count(),
                                    Reshoots = g.OrderByDescending(p => p.ReportDate).FirstOrDefault().RGReportRows.Where(p => p.Remark != null).Where(p => p.Remark.Value == "RESHOOT").Count(),
                                    Status = g.OrderByDescending(p => p.ReportDate).FirstOrDefault().Status.Status
                                }).ToList();

            return from r in intermediate
                   select new RTStatusReportRow
                   {
                       ID = r.ID,
                       FPNo = r.FPNo,
                       RTNo = r.RTNo,
                       Date = r.Date.ToString("dd/MM/yyyy"),
                       Repairs = r.Repairs.ToString(),
                       Retakes = r.Retakes.ToString(),
                       Reshoots = r.Reshoots.ToString(),
                       Status = r.Status
                   };
        }


        public FinalRTReport GetFinalRTReport(string rtNo)
        {
            //get the latest report in the sequence
            //can't use Last() here, have to use first() since this gets converted into a store query
            var rgReport = DbContext.RGReports.Where(p => p.RTNo == rtNo).Include(p => p.FixedPattern.Customer).Include(p=>p.Status)
                                .OrderByDescending(p => p.ID).FirstOrDefault();

            FinalRTReport finalReport = new FinalRTReport();
            rgReport.CopyTo(finalReport, "ReportDate,DateOfTest"); //since they are of different types it could cause issues
            finalReport.DateOfTest = rgReport.DateOfTest.ToString("dd-MM-yyyy");
            finalReport.ReportDate = rgReport.ReportDate.ToString("dd-MM-yyyy");

            List<FinalRTReportRow> finalRows = new List<FinalRTReportRow>();

            //get the latest rows for all locations for this rt no

            var reportRows = (from r in this.DbContext.RGReportRows
                              where r.RGReport.RTNo == rtNo
                              group r by new { r.Location, r.Segment } into g
                              //latest row for each combination
                              let latest = g.Where(p => p.Remark != null)
                                             .OrderByDescending(p => p.RGReport.ReportDate)
                                             .FirstOrDefault() ??
                                  //just to handle a case where the remark hasn't been filled yet for this location-segment
                                           g.OrderByDescending(p => p.RGReport.ReportDate)
                                             .FirstOrDefault()
                              select latest).Include(p => p.FilmSize).Include(p => p.Energy).ToList();

            int slno = 1;
            foreach (var r in reportRows)
            {
                FinalRTReportRow row = new FinalRTReportRow();
                r.CopyTo(row, string.Empty);
                //set parent id
                row.FinalRTReportID = finalReport.ID;
                //sl no should be independent
                row.SlNo = slno++;
                finalRows.Add(row);
            }

            finalReport.FinalRTReportRows = finalRows;

            return finalReport;
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

        #region RGStatus

        public IQueryable<RGStatus> GetRGStatuses()
        {
            return this.DbContext.RGStatuses;
        }

        #endregion
        
        #region RGRowTypes

        public IQueryable<RGReportRowType> GetRGRowTypes()
        {
            return this.DbContext.RGReportRowTypes;
        }

        #endregion

        #region RG Report Rows
        public IQueryable<RGReportRow> GetRGReportRows()
        {
            return this.DbContext.RGReportRows;
        }

        public IQueryable<RGReportRow> GetRGReportRowsByReportNo(string ReportNo)
        {
            return this.DbContext.RGReportRows.Where(p => p.RGReport.ReportNo == ReportNo);
        }


        public IQueryable<RGReportRow> GetRGReportRowsByFPNo(string fpNo)
        {
            return this.DbContext.RGReportRows.Where(p => p.RGReport.FixedPattern.FPNo == fpNo);
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
            //to avoid EF exception. It will depend on the fk ids to map the relationships correctly
            currentRGReportRow.FilmSize = null;
            currentRGReportRow.Technician = null;
            currentRGReportRow.Welder = null;
            currentRGReportRow.Remark = null;

            //if rgreportid is 0 then this row should be deleted not updated

            if (currentRGReportRow.RGReportID == 0)
            {
                DeleteRGReportRow(currentRGReportRow);
                return;
            }


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

        #region Roles

        public IEnumerable<String> GetRoles()
        {
            return Roles.GetAllRoles();
        }

        #endregion

        #region Shifts
        public IQueryable<Shift> GetShifts()
        {
            return this.DbContext.Shifts;
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

        /// <summary>
        /// Gets the shiftwise performance report of retake percentages by number and area
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="technicianId"></param>
        /// <returns></returns>
        public IEnumerable<ShiftWisePerformanceRow> GetShiftWisePerformanceReport(DateTime fromDate, DateTime toDate, int technicianId = -1)
        {
            fromDate = fromDate.Date;
            toDate = toDate.Date.AddDays(1);

            var retake = this.DbContext.Remarks.Single(p => p.Value == "RETAKE");

            //fetch from database, grouping can cause include to fail silently
            var intermediate1 = (from r in this.DbContext.RGReportRows
                                    .Include("FilmSize")
                                    .Include("Technician")
                                    .Include("Remark")
                                    .Include("RGReport")
                                    .Include("RGReport.Shift")
                                orderby r.RGReport.ReportDate, r.RGReport.Shift
                                where r.Technician.ID == (technicianId == -1 ? r.Technician.ID : technicianId)
                                && r.RGReport.ReportDate >= fromDate && r.RGReport.ReportDate <= toDate
                                select r).ToList();

            var intermediate2 = from r in intermediate1
                                group r by new
                                {
                                    Date = r.RGReport.ReportDate.Date,
                                    r.RGReport.Shift
                                } into g
                                select g;

            //process and structure
            var report = (from g in intermediate2
                         let total = g.Count()
                         let retakeCollection = g.Where(p => p.Remark != null).Where(p => p.Remark.ID == retake.ID)
                         let retakes = retakeCollection.Count()
                         let totalArea = g.Sum(p => p.FilmSize.Area)
                         let retakeArea = retakeCollection.Sum(p => p.FilmSize.Area)
                         select new ShiftWisePerformanceRow
                         {
                             ID = Guid.NewGuid(),
                             Technicians = String.Join(",", g.Select(p => p.Technician.Name).Distinct().ToList()),
                             Date = g.Key.Date.ToString("dd-MM-yyyy"),
                             Shift = g.Key.Shift.Value,
                             FilmAreaRows = (from f in g
                                            group f by f.FilmSize into fg
                                            select new FilmAreaRow
                                            {
                                                ID = Guid.NewGuid(),
                                                FilmSize = fg.Key.Name,
                                                Total = fg.Count(),
                                                RT = fg.Where(p => p.Remark != null).Where(p => p.Remark.ID == retake.ID).Count()
                                            }).ToList(),
                             TotalFilmsTaken = total,
                             TotalRetakes = retakes,
                             RTPercent = (retakes * 10000 / total) /100.0,
                             RTPercentByArea = (retakeArea * 10000 / totalArea) / 100.0
                         }).ToList();

            //fill parent guids in child rows
            foreach (var row in report)
            {
                foreach (var fa in row.FilmAreaRows)
                {
                    fa.ShiftWisePerformanceRowID = row.ID;
                }
            }

            return report;
        }
        #endregion

        #region Thickness Ranges
        public IQueryable<ThicknessRangeForEnergy> GetThicknessRangesForEnergy(String filter = "")
        {
            //check if it can be parsed to int, if so search based on thickness too
            int filterInt;
            if (Int32.TryParse(filter, out filterInt))
            {
                return this.DbContext.ThicknessRangesForEnergy
                        .Where(p => p.Energy.Name.Contains(filter) || (p.ThicknessFrom < filterInt && p.ThicknessTo > filterInt));
            }

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


        #region FileUpload

        public IQueryable<UploadedFile> GetUploadedFiles()
        {
            return this.DbContext.UploadedFiles;
        }

        public void InsertUploadedFile(UploadedFile entity)
        {
            DbEntityEntry<UploadedFile> entityEntry = this.DbContext.Entry(entity);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.UploadedFiles.Add(entity);
            }
        }



        [Invoke]
        public int UploadFile(string fileName, string fileType, string fileExtension, UInt64 fileSize, byte[] fileData)
        {
            UploadedFile file = new UploadedFile();
            file.FileName = fileName;
            file.FileType = fileType;
            file.FileExtension = fileExtension;
            file.FileSize = fileSize;
            file.FileData = fileData;

            this.DbContext.UploadedFiles.Add(file);
            this.DbContext.SaveChanges();

            return file.ID;            
        }

        #endregion
    }
}