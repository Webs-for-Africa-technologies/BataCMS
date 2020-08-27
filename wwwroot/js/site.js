"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/signalserver").build();


connection.on("updatePurchase", function (orderCount) {
    console.log("Purhases have been updated" + orderCount);
    localStorage.setItem("orderCount", orderCount);
    var Orders = document.getElementById("orderCount");
    Orders.innerHTML = orderCount;
    });

    connection.start();

