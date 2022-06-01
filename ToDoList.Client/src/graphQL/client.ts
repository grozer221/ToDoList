import {ApolloClient, InMemoryCache} from '@apollo/client';
import {schema} from './schema';

export const client = new ApolloClient({
    uri: !process.env.NODE_ENV || process.env.NODE_ENV === 'development' ? process.env.REACT_APP_GRAPH_QL_API_URL : '/graphql',
    cache: new InMemoryCache(),
    credentials: 'include',
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
