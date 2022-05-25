import {ApolloClient, HttpLink, InMemoryCache} from '@apollo/client';
import {schema} from './schema';

console.log('REACT_APP_GRAPH_QL_API_URL', process.env.REACT_APP_GRAPH_QL_API_URL)

export const client = new ApolloClient({
    link: new HttpLink({uri: process.env.REACT_APP_GRAPH_QL_API_URL}),
    cache: new InMemoryCache(),
    defaultOptions: {
        watchQuery: {
            // fetchPolicy: 'network-only',
            errorPolicy: 'all',
            notifyOnNetworkStatusChange: true,
        },
        query: {
            // fetchPolicy: 'network-only',
            errorPolicy: 'all',
            notifyOnNetworkStatusChange: true,
        },
    },
    typeDefs: schema,
});
