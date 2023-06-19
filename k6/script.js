import { check, group, sleep } from 'k6';
import http from 'k6/http';

export let options = {
  max_vus: 23,
  vus: 23,
  stages: [
    { duration: "30s", target: 10 },
    { duration: "1m", target: 23 },
    { duration: "30s", target: 0 }
  ]
}

export default function() {
  group('v1 API testing', function() {

    group('register', function() {
      let username = `test_user_${__VU}`;

      // Define the request payload
      let payload = {
        username: username,
        password: "test_password",
      };

      // Set the request headers
      let headers = {
        'Content-Type': 'application/json',
      };

      // Send the POST request
      let res = http.post('http://localhost:5198/api/Auth/register', JSON.stringify(payload), { headers: headers });

      check(res, {
        "status is 200": (r) => r.status === 200
      });
    });

  });
  sleep(1);
}