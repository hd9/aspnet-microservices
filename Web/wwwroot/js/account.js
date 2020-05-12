// newsletter
const nlApp = new Vue({
    el: '#nlApp',
    data: {
        submitted: false,
        name: '',
        email: ''
    },
    methods: {
        submit: function () {
            if (this.name.length < 5 || this.email.length < 5) {
                alert("Names and emails should have at least 5 characters");
                return false;
            }

            axios
                .post('/signup', { Name: this.name, Email: this.email })
                .then(r => { this.submitted = true; })
                .catch(error => console.log(error));

            this.submitted = true;
        },
        resubmit: function () {
            this.submitted = false;
            this.name = '';
            this.email = '';
        }
    }
});

// products
const productsApp = new Vue({
    el: '#prodApp',
    data: {
        products: []
    },
    methods: {
        view: function () {
            alert('todo');
        }
    },
    mounted() {
        if (!this.$refs.prod)
            return;

        axios.get('/products')
            .then(function (r) {
                if (r && r.data) {
                    r.data.forEach(p => {
                        productsApp.products.push(p);
                    });
                }
            })
            .catch(function (error) {
                console.log(error);
            });
    }
});


//edit account
const editAcctApp = new Vue({
    el: '#editAcctApp',
    data: {
        editMode: false
    },
    methods: {
        edit: function () {
            this.editMode = true;
        },
        cancel: function (e) {
            this.editMode = false;
            console.log(e);
            e.preventDefault();
        }
    }
});

// my orders
const myOrdersApp = new Vue({
    el: '#ordersApp',
    data: {
        hasOrders: false,
        orders: []
    },
    methods: {
        statusName: function (status) {
            return status === 0 ? 'New' : 'Submitted';
        }
    },
    mounted() {
        if (!this.$refs.orders)
            return;

        axios.get('/api/account/orders')
            .then(function (r) {
                if (r && r.data) {
                    r.data.forEach(o => {
                        myOrdersApp.hasOrders = true;
                        o.statusName = myOrdersApp.statusName(o.status);
                        myOrdersApp.orders.push(o);
                    });
                }
            })
            .catch(function (error) {
                console.log(error);
            });
    }
})

var addrOptionsApp = new Vue({
    el: '#addrOptions',
    data: {
        addresses: []
    },
    methods: {
        remove: function (i) {
            if (confirm('Are you sure you want to remove this address?')) {
                var id = this.addresses[i].id;
                axios.delete('/api/account/address/' + id)
                    .then(function (r) {
                        addrOptionsApp.addresses.splice(i, 1);
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            }
        },
        makeDefault: function (i, e) {
            if (confirm('Are you sure you want to make this address primary?')) {
                var id = this.addresses[i].id;
                axios.put('/api/account/address/' + id)
                    .then(function (r) {
                        var arr = addrOptionsApp.addresses;
                        arr.forEach(a => a.isDefault = false);
                        arr[i].isDefault = true;
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            }

            e.preventDefault();
        },
        url: function (i) {
            var id = this.addresses[i].id;
            return '/account/address/edit/' + id;
        },
        useThis: function (i) {
            this.addresses.forEach(el => el.isDefault = false);
            this.addresses[i].isDefault = true;
        }
    },
    computed: {
        selectedId: function () {
            return this.addresses.find(el => el.isDefault).id;
        }
    },
    mounted() {
        if (!this.$refs.addrOptions)
            return;

        axios.get('/api/account/addresses')
            .then(function (r) {
                if (r && r.data) {
                    addrOptionsApp.addresses = r.data.map(r => r);
                }
            })
            .catch(function (error) {
                console.log(error);
            });
    }
});


var pmtOptionsApp = new Vue({
    el: '#pmtOptions',
    data: {
        pmtInfos: [],
        hasData: false
    },
    methods: {
        remove: function (i) {
            if (confirm('Are you sure you want to remove this payment method?')) {
                var id = this.pmtInfos[i].id;
                axios.delete('/api/account/payment/' + id)
                    .then(function (r) {
                        pmtOptionsApp.pmtInfos.splice(i, 1);
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            }
        },
        makeDefault: function (i, e) {
            if (confirm('Are you sure you want to make this payment method default?')) {
                var id = this.pmtInfos[i].id;
                axios.put('/api/account/payment/' + id)
                    .then(function (r) {
                        var arr = pmtOptionsApp.pmtInfos;
                        arr.forEach(a => a.isDefault = false);
                        arr[i].isDefault = true;
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            }
        },
        url: function (i) {
            var id = this.pmtInfos[i].id;
            return '/account/payment/edit/' + id;
        },
        expDate: function (dt) {
            var d = new Date(dt);
            return ("0" + (d.getMonth() + 1)).slice(-2) + '/' + d.getFullYear();
        },
        method: function (m) {
            switch (m) {
                case 2:
                    return 'AMEX';
                case 1:
                    return 'Visa';
                default:
                    return 'MasterCard';
            }
        },
        useThis: function (i) {
            this.pmtInfos.forEach(el => el.isDefault = false);
            this.pmtInfos[i].isDefault = true;
        }
    },
    computed: {
        selectedId: function () {
            var pmtInfo = this.pmtInfos.find(el => el.isDefault);
            return pmtInfo ? pmtInfo.id : null;
        }
    },
    mounted() {
        if (!this.$refs.pmtOptions)
            return;

        axios.get('/api/account/payments')
            .then(function (r) {
                if (r && r.data) {
                    pmtOptionsApp.pmtInfos = r.data.map(r => r);
                    pmtOptionsApp.hasData = r.data.length > 0;
                }
            })
            .catch(function (error) {
                console.log(error);
            });
    }
});

var checkoutCartApp = new Vue({
    el: '#checkoutCart',
    methods: {
        submit: function () {
            var addrId = addrOptionsApp.selectedId;
            var pmtId = pmtOptionsApp.selectedId;

            if (!addrId || !pmtId) {
                alert('Please, select an address and a payment method');
                return;
            }

            window.location = `/cart/review?addrId=${encodeURI(addrId)}&pmtId=${encodeURI(pmtId)}`;
        }
    }
})