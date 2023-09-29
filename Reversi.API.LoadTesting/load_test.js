import http from 'k6/http';

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    stages: [
        { duration: '5m', target: 100 }, // simulate ramp-up traffic from 1 to 100 users over 5 minutes..
        { duration: '10m', target: 100 }, // stay at 100 users for 10minutes.
        { duration: '5m', target: 0 }, // ramp-down to 0 users.
    ],
    thresholds: {
        http_req_duration: ['p(99)<150'],
    },
};

export default () => {
    http.get('https://localhost:44339/api/spel');
};