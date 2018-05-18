let userId = globalSettings.userId;
let serverUrl = globalSettings.serverBaseUrl;
if (!serverUrl.endsWith('/')) {
    serverUrl = serverUrl + '/';
}

let createEmptyOrderModel = function() {
    return { userId: userId, isBuyOrder: 'True' };
}

let app = new Vue({
    el: '#app',
    data: {
        newOrder: createEmptyOrderModel(),
        orders: [],
        trades: []
    },
    mounted: function () {
        this.$on('newTrades', function (newTrades) {
            for (let t of newTrades) {
                this.updateOrder(t.BuyOrder);
                this.updateOrder(t.SellOrder);
                if (t.BuyOrder.UserId == userId || t.SellOrder.UserId == userId) {
                    this.trades.push(t);                    
                }
            }
        })
        this.$on('newOrders', function (newOrders) {            
            for (let o of newOrders) {
                this.orders.push(o);
            }
        });
    },
    methods: {
        updateOrder: function (updatedOrder) {
            for (let o of this.orders) {
                if (o.OrderId == updatedOrder.OrderId) {
                    o.RemainingCount = updatedOrder.RemainingCount
                }
            }
        },
        submitOrder: function () {
            let local = this;
            let newOrder = this.newOrder;
            axios
                .post(serverUrl + 'api/orderbook/add', newOrder)
                .then(function (response) {
                    local.newOrder = createEmptyOrderModel();
                })
                .catch(function (error) {
                    console.log(error);
                });
        },
        onNewTrades: function (trades) {
            console.log(trades);
        }
    }
})