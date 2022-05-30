import React, {useEffect} from 'react';
import {useDispatch, useSelector} from "react-redux";
import {RootState} from "../../../store/store";
import {useNavigate, useParams} from "react-router-dom";
import {categoriesActions} from "../../../store/categories/categories.actions";
import {Form, Input, message} from 'antd';
import Title from "antd/lib/typography/Title";
import {ButtonSubmit} from "../../../components/ButtonSubmit/ButtonSubmit";
import {Loading} from "../../../components/Loading/Loading";
import {CategoriesUpdateInputType} from "../../../graphQL/modules/categories/categories.mutations";

export const CategoriesUpdate = () => {
    const params = useParams();
    const inUpdateCategoryId = parseInt(params.id || '') || 0;
    const inUpdateCategory = useSelector((s: RootState) => s.categories.inUpdateCategory)
    const fetchInUpdateCategoryError = useSelector((s: RootState) => s.categories.fetchInUpdateCategoryError)
    const fetchInUpdateCategoryLoading = useSelector((s: RootState) => s.categories.fetchInUpdateCategoryLoading)
    const fetchUpdateCategoryError = useSelector((s: RootState) => s.categories.fetchUpdateCategoryError)
    const fetchUpdateCategoryLoading = useSelector((s: RootState) => s.categories.fetchUpdateCategoryLoading)
    const categories = useSelector((s: RootState) => s.categories.categories)
    const navigateTo = useSelector((s: RootState) => s.categories.navigateTo)
    const dispatch = useDispatch();
    const navigate = useNavigate();

    useEffect(() => {
        const updateCategory = categories.find(t => t.id === inUpdateCategoryId);
        if (updateCategory) {
            dispatch(categoriesActions.setInUpdateCategory(updateCategory));
            dispatch(categoriesActions.setFetchInUpdateCategoryLoading(false));
        } else {
            dispatch(categoriesActions.fetchInUpdateCategory(inUpdateCategoryId));
        }

        return () => {
            dispatch(categoriesActions.resetAfterLeaveUpdatePage());
        }
    }, [])

    useEffect(() => {
        if (fetchInUpdateCategoryError)
            message.error(fetchInUpdateCategoryError);
        if (fetchUpdateCategoryError)
            message.error(fetchUpdateCategoryError)
    }, [fetchInUpdateCategoryError, fetchUpdateCategoryError])

    const onFinish = async (categoriesUpdateInputType: CategoriesUpdateInputType) => {
        dispatch(categoriesActions.fetchUpdateCategory(categoriesUpdateInputType))
    };

    if (fetchInUpdateCategoryLoading)
        return <Loading/>

    if (navigateTo) {
        const navigateToCopy = navigateTo;
        dispatch(categoriesActions.setNavigateTo(''));
        navigate(navigateToCopy);
    }

    return (
        <Form
            name="CategoryUpdateForm"
            onFinish={onFinish}
            initialValues={{
                id: inUpdateCategory?.id,
                name: inUpdateCategory?.name,
            }}
        >
            <Title level={2}>Update category</Title>
            <Form.Item name="id" style={{display: 'none'}}>
                <Input type={'hidden'}/>
            </Form.Item>
            <Form.Item
                name="name"
                label="Name"
                rules={[{required: true, message: 'Name is required!'}]}
            >
                <Input placeholder="Name"/>
            </Form.Item>
            <Form.Item>
                <ButtonSubmit loading={fetchUpdateCategoryLoading} isSubmit={true}>Update</ButtonSubmit>
            </Form.Item>
        </Form>
    );
};