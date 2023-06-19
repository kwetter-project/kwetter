import { check, group, sleep } from 'k6';
import http from 'k6/http';

export let options = {
  max_vus: 120,
  vus: 120,
  stages: [
    { duration: "30s", target: 10 },
    { duration: "4m", target: 120 },
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

  // Create a tweet
  createTweet(username, authToken);

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
    if (response.status === 201) {
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

  // Extract the authorization token from the response header
  let authToken = response.headers['Authorization'];

  // Validate the response and extract the authorization token
  if (response.status === 200 && authToken) {
    console.log(`User ${username} logged in successfully.`);
    return authToken.replace('Bearer ', '');
  } else {
    console.error(`Failed to log in user ${username}. Status code: ${response.status}`);
    return null;
  }
}

function createTweet(username, authToken) {
  // Define the request payload
  let payload = {
    username: username,
    type: "tweet",
    content: "test",
  };

  // Set the request headers
  let headers = {
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${authToken}`,
  };

  // Send the POST request to create a tweet
  let response = http.post('http://localhost:5195/api/Tweet', JSON.stringify(payload), { headers: headers });

  // Validate the response
  if (response.status === 201) {
    console.log(`Tweet created successfully for user ${username}.`);
  } else {
    console.error(`Failed to create tweet for user ${username}. Status code: ${response.status}`);
  }
}