const STRINGEE_SERVER_ADDRS = [
    "wss://v1.stringee.com:6899/",
    "wss://v2.stringee.com:6899/"
];
var stringeeClient = new StringeeClient(STRINGEE_SERVER_ADDRS);

let activeCall = null;
let isClientConnected = false;

document.addEventListener("DOMContentLoaded", function () {
    getTokenAndConnect();
});

stringeeClient.on('connect', function () {
    console.log('✅ Đã kết nối đến Stringee Server');
    isClientConnected = true;
});

stringeeClient.on('disconnect', function () {
    console.log('❌ Mất kết nối với Stringee Server');
    isClientConnected = false;
});

stringeeClient.on('error', function (err) {
    console.error("❌ Lỗi kết nối với Stringee:", err);
});

async function getTokenAndConnect() {
    try {
        const response = await fetch("/api/stringee/token?userId=test_user");
        const data = await response.json();
        const token = data.access_token;

        stringeeClient.connect(token);
    } catch (error) {
        console.error("Lỗi khi lấy token:", error);
    }
}
function makeCall() {
    if (!isClientConnected) {
        console.error("❌ Stringee Client chưa kết nối.");
        return;
    }

    const toNumber = document.getElementById("toNumber").value;
    if (!toNumber) {
        console.error("❌ Vui lòng nhập số điện thoại.");
        return;
    }

    const fromNumber = "02871020549";
    activeCall = new StringeeCall(stringeeClient, fromNumber, toNumber, false);

    activeCall.on('signalingStateChange', function (state) {
        console.log("📞 Trạng thái cuộc gọi:", state);
    });

    activeCall.on('error', function (error) {
        console.error("❌ Lỗi cuộc gọi:", error);
    });

    activeCall.makeCall(function (res) {
        if (res.r === 0) {
            console.error("❌ Không thể thực hiện cuộc gọi:", res);
        } else {
            console.log("✅ Cuộc gọi bắt đầu:", res);
        }
    });
}

function endCall() {
    if (activeCall) {
        activeCall.hangup(function (res) {
            if (res.r === 0) {
                console.error("❌ Lỗi khi kết thúc cuộc gọi:", res);
            } else {
                console.log("✅ Cuộc gọi đã kết thúc:", res);
                activeCall = null;
            }
        });
    } else {
        console.error("❌ Không có cuộc gọi nào đang diễn ra.");
    }
}
