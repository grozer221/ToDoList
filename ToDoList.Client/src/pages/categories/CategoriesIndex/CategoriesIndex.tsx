import React, {useCallback, useEffect} from 'react';
import {Link, useSearchParams} from "react-router-dom";
import {useDispatch, useSelector} from "react-redux";
import {RootState} from "../../../store/store";
import {categoriesActions} from "../../../store/categories/categories.actions";
import {Col, Form, message, Row, Select, Space, Table} from "antd";
import {ColumnsType} from "antd/es/table";
import {ButtonsVUR} from "../../../components/ButtonsVUD/ButtonsVUR";
import Title from 'antd/lib/typography/Title';
import Search from "antd/es/input/Search";
import debounce from 'lodash.debounce';
import {Category} from "../../../graphQL/modules/categories/categories.types";
import {CategoriesSortOrder} from "../../../graphQL/enums/categoriesSortOrder";
import {ButtonSubmit} from "../../../components/ButtonSubmit/ButtonSubmit";
import {camelCaseToString} from "../../../convertors/stringToDatetimeConvertors";
import {Loading} from "../../../components/Loading/Loading";

export const CategoriesIndex = () => {
    const dispatch = useDispatch();
    const pageSize = useSelector((s: RootState) => s.categories.pageSize)
    const total = useSelector((s: RootState) => s.categories.total)
    const categories = useSelector((s: RootState) => s.categories.categories)
    const fetchCategoriesLoading = useSelector((s: RootState) => s.categories.fetchCategoriesLoading)
    const fetchCategoriesError = useSelector((s: RootState) => s.categories.fetchCategoriesError)
    const fetchRemoveCategoryError = useSelector((s: RootState) => s.categories.fetchRemoveCategoryError)
    const [searchParams, setSearchParams] = useSearchParams();

    useEffect(() => {
        if (fetchCategoriesError) {
            message.error(fetchCategoriesError);
            dispatch(categoriesActions.setFetchCategoriesError(''));
        }
        if (fetchRemoveCategoryError) {
            message.error(fetchRemoveCategoryError);
            dispatch(categoriesActions.setFetchRemoveCategoryError(''));
        }
    }, [fetchCategoriesError, fetchRemoveCategoryError])

    useEffect(() => {
        const page = parseInt(searchParams.get('page') || '') || 1
        const likeInput = searchParams.get('like');
        const sortOrderString = searchParams.get('sortOrder');
        const sortOrder = CategoriesSortOrder[sortOrderString as keyof typeof CategoriesSortOrder] || CategoriesSortOrder.nameAsc;
        dispatch(categoriesActions.fetchCategories(page, likeInput, sortOrder));
    }, [searchParams])

    const onRemove = (id: number): void => {
        dispatch(categoriesActions.fetchRemoveCategory(id));
    }

    const columns: ColumnsType<Category> = [
        {
            title: 'Name',
            dataIndex: 'name',
            key: 'name',
        },
        {
            title: 'Actions',
            dataIndex: 'actions',
            key: 'actions',
            width: '130px',
            render: (text, category) => (
                <ButtonsVUR updateUrl={`update/${category.id}`} onRemove={() => onRemove(category.id)}/>
            ),
        },
    ];

    const debouncedSearchCategoriesHandler = useCallback(debounce(like => setSearchParams({like}), 500), []);
    const searchCategoriesHandler = (value: string) => {
        debouncedSearchCategoriesHandler(value);
    };

    if (total === 0 && pageSize === 0 && fetchCategoriesLoading)
        return <Loading/>

    return (
        <Space direction={'vertical'} style={{width: '100%'}}>
            <Title level={2}>Categories</Title>
            <Link to={'create'}>
                <ButtonSubmit isSubmit={false}>Create</ButtonSubmit>
            </Link>
            <Form name="CategoriesFilterForm">
                <Row>
                    <Col span={10}>
                        <Form.Item name="like">
                            <Search
                                defaultValue={searchParams.get('like') || ''}
                                onChange={e => searchCategoriesHandler(e.target.value)}
                                placeholder="Search"
                                enterButton
                                loading={fetchCategoriesLoading}
                                allowClear={true}
                            />
                        </Form.Item>
                    </Col>
                    <Col span={7}>
                        <Form.Item name="Sort order">
                            <Select
                                defaultValue={searchParams.get('sortOrder') || CategoriesSortOrder.nameAsc}
                                placeholder="Sorting"
                                onChange={sortOrder => setSearchParams({sortOrder})}
                            >
                                {(Object.keys(CategoriesSortOrder) as Array<keyof typeof CategoriesSortOrder>).map(key => (
                                    <Select.Option value={key} key={key}>{camelCaseToString(key)}</Select.Option>
                                ))}
                            </Select>
                        </Form.Item>
                    </Col>
                </Row>
            </Form>
            <Table
                rowKey={'id'}
                dataSource={categories}
                columns={columns}
                loading={fetchCategoriesLoading}
                pagination={{
                    current: parseInt(searchParams.get('page') || '') || 1,
                    defaultPageSize: pageSize,
                    total: total,
                    onChange: page => setSearchParams({page: page.toString()}),
                }}
            />

        </Space>
    );
};