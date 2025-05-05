//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using NeoIsisJob.Repositories;
//using NeoIsisJob.Repositories.Interfaces;
//using NeoIsisJob.Models;

//namespace Tests.Repo.Mocks
//{
//    public class RankingsMock : IRankingsRepository
//    {
//        private List<RankingModel> rankings;
//        public RankingsMock() { 
//            rankings = new List<RankingModel>();
//            rankings.Add(new RankingModel(1, 1, 1)); //uid, mgid, rank
//            rankings.Add(new RankingModel(1, 2, 2));
//            rankings.Add(new RankingModel(1, 3, 3));
//        }

//        public IList<RankingModel> GetAllRankingsByUserID(int userId)
//        {
//            return rankings.Where(r => r.UserId == userId).ToList();
//        }
//        public RankingModel GetRankingByFullID(int userId, int muscleGroupId)
//        {
//            return rankings.FirstOrDefault(r => r.UserId == userId && r.MuscleGroupId == muscleGroupId);
//        }
        


//    }
//}