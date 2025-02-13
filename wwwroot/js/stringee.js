

const STRINGEE_SERVER_ADDRS = [
    "wss://v1.stringee.com:6899/",
    "wss://v2.stringee.com:6899/"
];
var stringeeClient = new StringeeClient(STRINGEE_SERVER_ADDRS);

const fromNumbers = [
    { alias: 'Number-1', number: '842871020549' }
];

document.addEventListener("DOMContentLoaded", function () {
    console.log('DOMContentLoaded')
    getTokenAndConnect('test_user', 1);
   //getTokenAndConnect('user1', 1); // test receive call
});

stringeeClient.on('connect', function () {
    console.log('Connected to Stringee Server');
    isClientConnected = true;
});

stringeeClient.on('authen', function (res) {
    console.log('authen: ', res);
});

//stringeeCl

stringeeClient.on('incomingcall', function (incomingcall) {
    console.log('incomingcall:', incomingcall);
    $('#incoming-call-notice').show();

    incomingcall.on('signalingstate', function (state) {
        console.log('status state:', state);
    });
});

stringeeClient.on('disconnect', function () {
    console.log('Disconnected from Stringee Server');
    isClientConnected = false;
});

stringeeClient.on('error', function (err) {
    console.error("Connection error with Stringee:", err);
});

async function getTokenAndConnect(userId, tokenActionType) {
    try {
        const requestData = {
            userId: userId,
            actionType: tokenActionType
        };

        const response = await fetch(`/api/stringee/generate_token`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(requestData)
        });

        if (!response.ok) {
            console.error(error);
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        currentToken = await response.text();
        console.log("Token:", currentToken);

        stringeeClient.connect(currentToken);
        return currentToken

    } catch (error) {
        console.error("Error while fetching token:", error);
    }
}

function makeCall(type) {
    if (!isClientConnected) {
        console.error("Stringee Client is not connected.");
        return;
    }

    const toNumber = document.getElementById("toNumber").value.trim();
    if (!toNumber) {
        console.error("Please enter a phone number.");
        return;
    }

    const fromNumber = fromNumbers.length > 0 ? fromNumbers[0] : null;
    if (!fromNumber) {
        console.error("No valid fromNumber available.");
        return;
    }

    console.log(`📞 Calling from: ${fromNumber.number} to: ${toNumber}`);

    activeCall = new StringeeCall(stringeeClient, fromNumber.number, toNumber, false);

    activeCall.makeCall(function (res) {

        console.log('makeOutgoingCallBtnClick: fromNumber=' + fromNumber.number + ', toNumber=' + toNumber);

        if (res.r == 0) {
            console.log("Call started:", res);
            callId = res.callId;
            console.log(callId);

            return res.message;
        } else {
            const message = `Unable to make the call: ${res.message}`;
            console.error(message);
            console.log(message);
            return res.message;
        }
    });

    activeCall.on('signalingstate', function (state) {
        console.log('signalingstate ', state);

        if (state.code === 6 || state.code === 5) {
            $('#incoming-call-notice').hide();
        }
    });

    activeCall.on('mediastate', function (state) {
        console.log('Mediastate ', state);
    });

    activeCall.on('info', function (info) {
        console.log('on info:' + JSON.stringify(info));
    });

    //activeCall.on('addremotestream', function (state) {
    //    console.log('Addremotestream:', state)
    //}); để thây được video của người in call

    //activeCall.on('addlocalstream', function (state) {
    //    console.log('Addlocalstream:', state)
    //}); truyền video call của bản thân
}

async function login() {
    try {
        const body = {
            email: "heoninh47@gmail.com",
            password: "",
        }

        const response = await fetch('/api/stringee/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(body)
        });

        if (!response.ok) {
            throw new Error('Login failed');
        }

        const data = await response.json();
        const token = data.token;
        ;
        return token;
    } catch (error) {
        console.error('Error during login:', error);
    }
}

async function endCall() {

    if (activeCall) {
        activeCall.hangup(async function (res) {
            if (res.r != 0) {
                console.error("Error ending the call:", res.message);
            } else {

                console.log("Call ended:");
                activeCall = null;

                document.getElementById("incoming-call-notice").style.display = "none";
            }
        });
    } else {
        console.error("No ongoing call to end.");
    }
}

async function downloadRecording() {
    const token = await getTokenAndConnect('test_user', 2);
    const callId = document.getElementById("callId").value.trim();

    if (!callId || !token) {
        console.error("Call ID and Access Token are required.");
        return;
    }

    try {
        const response = await fetch(`/api/stringee/download-recording?callId=${callId}&accessToken=${token}`, {
            method: 'GET',
            headers: { Accept: 'application/octet-stream' } 
        });

        if (!response.ok) {
            const errorDetails = await response.text();
            throw new Error(`Failed to download the recording: ${response.status} - ${errorDetails}`);
        }

        console.log('Download request successful');

        const data = await response.blob();
        const url = window.URL.createObjectURL(data);

        const a = document.createElement('a');
        a.href = url;
        a.download = `recording_${callId}.mp3`;
        document.body.appendChild(a);
        a.click();

        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
    } catch (error) {
        console.error("Error downloading recording:", error);
    }
}


async function callLog() {
    const token = await getTokenAndConnect('test_user', 2);
    const callIdCheck = document.getElementById("callId").value.trim();

    if (!callIdCheck || !token) {
        console.error("Call ID and Access Token are required.");
        return;
    }

    try {
        const response = await fetch(`/api/call/log?callId=${callIdCheck}&accessToken=${token}`, {
            method: 'GET',
        });

        console.log(response);

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        document.getElementById("call-log-container").style.display = "block"; 
        document.getElementById("call-log-detail").textContent = JSON.stringify(data.data, null, 4); 
    } catch (error) {
        console.error("Error fetching call log:", error);
        document.getElementById("call-log-container").style.display = "block";
        document.getElementById("call-log-detail").textContent = "Error fetching call log.";
    }

}