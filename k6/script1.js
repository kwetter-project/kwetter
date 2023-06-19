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

export default function () {
  // Generate a random username for each virtual user
  let username = `test_user_${__VU}`;

  // Create a new account
  createUser(username);

  // Log in to obtain authentication token
  let authToken = login(username);

  // Delete the account
  deleteAccount(username, authToken);

  // Sleep for 1 second between requests
  sleep(1);
}

function createUser(username) {
  // Define the request payload
  let payload = {
    username: username,
    password: "test_password",
  };

  // Set the request headers
  let headers = {
    'Content-Type': 'application/json',
  };

  // Send the POST request to create a new account
  let response = http.post('http://localhost:5198/api/Auth/register', JSON.stringify(payload), { headers: headers });

  // Validate the response
  if (response.status === 200) {
    console.log(`User ${username} created successfully.`);
  } else {
    console.error(`Failed to create user ${username}. Status code: ${response.status}`);
  }
}

function login(username) {
  // Define the request payload
  let payload = {
    username: username,
    password: "test_password",
  };

  // Set the request headers
  let headers = {
    'Content-Type': 'application/json',
  };

  // Send the POST request to login
  let response = http.post('http://localhost:5198/api/Auth/login', JSON.stringify(payload), { headers: headers });
  let authToken = response.headers['Authorization'];
  // Validate the response
  if (response.status === 200) {
    console.log(`User ${username} logged in successfully.`);
    return authToken.replace('Bearer ', '');
  } else {
    console.error(`Failed to log in user ${username}. Status code: ${response.status}`);
    return null;
  }
}

function deleteAccount(username, authToken) {
  // Set the request headers
  let headers = {
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${authToken}`,
  };

  // Send the DELETE request to delete the account
  let response = http.del(`http://localhost:5198/api/Auth/${username}`, null, { headers: headers });

  // Validate the response
  if (response.status === 204) {
    console.log(`User ${username} deleted successfully.`);
  } else {
    console.error(`Failed to delete user ${username}. Status code: ${response.status}`);
  }
}