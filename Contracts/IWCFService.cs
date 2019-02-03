using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Contracts
{
    [ServiceContract]
    public interface IWCFService
    {
        [OperationContract]
        Entity Read(int id);

        [OperationContract]
        int Edit(int monthNo, int monthlyConsumption, int idOfEntity);

        [OperationContract]
        int Add(int id, string region, string city, int year, List<int> consumption);

        [OperationContract]
        double Calculate(string city);

        [OperationContract]
        int Delete(int id);
    }
}
