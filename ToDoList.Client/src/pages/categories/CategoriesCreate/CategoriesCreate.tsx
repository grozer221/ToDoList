import React, {useEffect} from 'react';
import {useDispatch, useSelector} from "react-redux";
import {Form, Input, message} from "antd";
import Title from "antd/lib/typography/Title";
import {ButtonSubmit} from "../../../components/ButtonSubmit/ButtonSubmit";
import {RootState} from "../../../store/store";
import {CategoriesCreateInputType} from "../../../graphQL/modules/categories/categories.mutations";
import {categoriesActions} from "../../../store/categories/categories.actions";
import {useNavigate} from "react-router-dom";

export const CategoriesCreate = () => {
    const fetchCreateCategoryError = useSelector((s: RootState) => s.categories.fetchCreateCategoryError)
    const fetchCreateCategoryLoading = useSelector((s: RootState) => s.categories.fetchCreateCategoryLoading)
    const navigateTo = useSelector((s: RootState) => s.categories.navigateTo)
    const dispatch = useDispatch();
    const navigate = useNavigate();

    useEffect(() => {
        if (fetchCreateCategoryError) {
            message.error(fetchCreateCategoryError);
            dispatch(categoriesActions.setFetchCreateCategoryError(''));
        }
    }, [fetchCreateCategoryError])

    const onFinish = (toDosCreateInputType: CategoriesCreateInputType): void => {
        dispatch(categoriesActions.fetchCreateCategory({...toDosCreateInputType}))
    }

    if (navigateTo) {
        const navigateToCopy = navigateTo;
        dispatch(categoriesActions.setNavigateTo(''));
        navigate(navigateToCopy);
    }

    return (
        <Form
            name="CategoryCreateForm"
            onFinish={onFinish}
        >
            <Title level={2}>Create category</Title>
            <Form.Item
                name="name"
                label="Name"
                rules={[{required: true, message: 'Name is required!'}]}
            >
                <Input placeholder="Name"/>
            </Form.Item>
            <Form.Item>
                <ButtonSubmit loading={fetchCreateCategoryLoading} isSubmit={true}>Create</ButtonSubmit>
            </Form.Item>
        </Form>
    );
};