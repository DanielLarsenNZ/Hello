using Hello.Profile.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Hello.Profile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        static ConcurrentDictionary<string, User> _data = new ConcurrentDictionary<string, User>(
                new KeyValuePair<string, User>[]
        {
            new KeyValuePair<string, User> (
                "abc123",
                new User
                    {
                        Id ="abc123",
                        Name = "Alice Alison",
                        Username = "alice",
                        Email ="alice@localtest.me",
                        CreatedDate = DateTime.UtcNow,
                        ModifiedDate =DateTime.UtcNow
                    }
                ),
            new KeyValuePair<string, User> (
                "def456",
                new User
                    {
                        Id ="def456",
                        Name = "Bob Bobson",
                        Username = "bob",
                        Email ="bob@localtest.me",
                        CreatedDate = DateTime.UtcNow,
                        ModifiedDate =DateTime.UtcNow
                    }
                )
        });

        // GET: api/Users
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _data.Select(u => u.Value);
        }

        // GET: api/Users/5
        [HttpGet("{id}", Name = "Get")]
        public User Get(string id)
        {
            return _data[id];
        }

        // POST: api/Users
        [HttpPost]
        public ActionResult Post([FromBody] User user)
        {
            if (_data.TryAdd(user.Id, user)) return Ok();
            return Conflict();
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] User user)
        {
            if (_data.TryUpdate(id, user, user)) return Ok();
            return Conflict();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            User user;
            if (_data.TryRemove(id, out user)) return Ok();
            return Conflict();
        }
    }
}
