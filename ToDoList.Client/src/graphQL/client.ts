import {ApolloClient, createHttpLink, InMemoryCache} from '@apollo/client';
import {schema} from './schema';

const link = createHttpLink({
    uri: !process.env.NODE_ENV || process.env.NODE_ENV === 'development' ? process.env.REACT_APP_GRAPH_QL_API_URL : '/graphql',
    credentials: 'include'
});

export const client = new ApolloClient({
    link,
    cache: new InMemoryCache(),
    defaultOptions: {
        watchQuery: {
            errorPolicy: 'all',
            notifyOnNetworkStatusChange: true,
        },
        query: {
            errorPolicy: 'all',
            notifyOnNetworkStatusChange: true,
        },
    },
    typeDefs: schema,
});
