let connection = new signalR.HubConnectionBuilder()
    .withUrl("/callHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

const webhookList = document.getElementById("webhook-list");
const webhookDetailContainer = document.getElementById("webhook-detail-container");
const webhookDetail = document.getElementById("webhook-detail");

let webhookDataList = [];

connection.on("ReceiveCallData", function (data) {
    //console.log("Webhook data received:", data);

    try {
        const jsonData = typeof data === "string" ? JSON.parse(data) : data;
        webhookDataList.push({
            time: new Date().toLocaleTimeString(),
            data: jsonData
        });

        updateWebhookList();
    } catch (error) {
        console.error("Error parsing JSON:", error);
    }
});

connection.start()
    .then(() => console.log("SignalR Connected"))
    .catch(err => console.error("SignalR Connection Error: ", err));

function updateWebhookList() {
    if (!webhookList) return;

    webhookList.innerHTML = "";

    webhookDataList.forEach((item, index) => {
        let listItem = document.createElement("li");
        listItem.className = "list-group-item";
        listItem.innerText = `Webhook #${index + 1} - ${item.time}`;
        listItem.style.cursor = "pointer";

        listItem.onclick = () => showWebhookDetail(index);

        webhookList.appendChild(listItem);
    });
}

function showWebhookDetail(index) {
    if (!webhookDetailContainer || !webhookDetail) return;

    webhookDetail.textContent = JSON.stringify(webhookDataList[index].data, null, 4);
    webhookDetailContainer.style.display = "block";
}
