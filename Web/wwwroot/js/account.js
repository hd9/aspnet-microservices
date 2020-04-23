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

    },
    mounted() {
        axios.get('/api/account/orders')
            .then(function (r) {
                if (r && r.data) {
                    r.data.forEach(o => {
                        myOrdersApp.hasOrders = true;
                        myOrdersApp.orders.push(o);
                    });
                }
            })
            .catch(function (error) {
                console.log(error);
            });
    }
})