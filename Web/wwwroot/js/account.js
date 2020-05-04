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

var addresses = new Vue({
    el: '#addresses',
    data: {
        addresses: []
    },
    methods: {
        remove: function (i) {
            if (confirm('Are you sure you want to remove this address?')) {
                var id = this.addresses[i].id;
                axios.delete('/api/account/address/' + id)
                    .then(function (r) {
                        addresses.addresses.splice(i, 1);
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            }
        },
        makeDefault: function (i) {
            if (confirm('Are you sure you want to make this address primary?')) {
                var id = this.addresses[i].id;
                axios.put('/api/account/address/' + id)
                    .then(function (r) {
                        var arr = addresses.addresses;
                        arr.forEach(a => a.isDefault = false);
                        arr[i].isDefault = true;
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            }
        },
        url: function (i) {
            var id = this.addresses[i].id;
            return '/account/address/edit/' + id;
        }
    },
    mounted() {
        if (!this.$refs.addresses)
            return;

        axios.get('/api/account/addresses')
            .then(function (r) {
                if (r && r.data) {
                    addresses.addresses = r.data.map(r => r);
                }
            })
            .catch(function (error) {
                console.log(error);
            });
    }
});