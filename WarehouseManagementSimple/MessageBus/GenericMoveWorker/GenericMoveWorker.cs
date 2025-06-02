using Core;
using DB;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBus.GenericMoveWorker
{
    public class GenericMoveWorker : Worker
    {
        private readonly DBLocation _dbLocation;
        private readonly DBStUnit   _dbStUnit;

        private readonly Location _fromLocation;
        private readonly Location _toLocation;

        bool _validConfig = true;

        public GenericMoveWorker(IMessageBus messageBus, Location fromLocation, Location toLocation) : base(messageBus)
        {
            SqliteConnection sqliteConnection = SqliteConnectionHelper.GetInstance();
            _dbLocation = new(sqliteConnection);
            _dbStUnit   = new(sqliteConnection);

            _fromLocation = fromLocation;
            _toLocation = toLocation;

            bool fromLocationFound = _dbLocation.Get(ref _fromLocation);
            bool toLocationFound = _dbLocation.Get(ref _toLocation);

            if (!_dbLocation.Get(ref _fromLocation))
            {
                Console.WriteLine($"Could not find from-location with Mha <{_fromLocation.Mha}>, Rack <{_fromLocation.Rack}>, HorCoor <{_fromLocation.HorCoor}> and VerCoor <{_fromLocation.VerCoor}>");
                _validConfig = false;
            }

            if (!_dbLocation.Get(ref _toLocation))
            {
                Console.WriteLine($"Could not find to-location with Mha <{_toLocation.Mha}>, Rack <{_toLocation.Rack}>, HorCoor <{_toLocation.HorCoor}> and VerCoor <{_toLocation.VerCoor}>");
                _validConfig = false;
            }
        }

        public GenericMoveWorker(IMessageBus messageBus, string fromMha, string toMha)
            : this(messageBus, new Location(){ Mha = fromMha}, new Location() { Mha = toMha})
        {
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && _validConfig)
            {
                foreach(StUnit? stUnit in _dbStUnit.GetAll().Where(s => s.FK_LocationID == _fromLocation.LocationID))
                {
                    if (stUnit == null)
                        continue;

                    stUnit.FK_LocationID = _toLocation.LocationID;
                    if (!_dbStUnit.Update(stUnit))
                    {
                        Console.WriteLine($"Failed to update location of StUnit <{stUnit.StUnitID}>");
                    }
                }
                await Task.Delay(1000);
            }
        }
    }
}
