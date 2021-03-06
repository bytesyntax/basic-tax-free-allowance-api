namespace BasicTaxFreeAllowanceApi.Models
{
    public class BTFARepository
    {
        private readonly Dictionary<string, BTFARecord> _records = new();

        public void Create(BTFARecord record)
        {
            if (record == null)
            {
                return;
            }

            _records[record.Id] = record;
        }

        public BTFARecord GetById(string id)
        {
            if (_records.ContainsKey(id))
                return _records[id];
            return null;
        }

        public List<BTFARecord> GetAll()
        {
            return _records.Values.ToList();
        }

        // Note: not part of requirements
/*        public void Update(BTFARecord record)
        {
            var existingRecord = GetById(record.Id);
            if (existingRecord == null)
            {
                return;
            }

            _records[record.Id] = record;
        }*/

        // Note: not part of requirements
/*        public void Delete(string id)
        {
            _records.Remove(id);
        }*/
    }
}
