import React, { useState, useEffect } from 'react'
import { Edit, Form, TextInput, DateInput, minValue, RadioButtonGroupInput, SimpleForm } from 'react-admin'
import { InputLabel, MenuItem, Select, Box, Button, Typography, Container, CssBaseline } from '@mui/material'
import { createTheme, ThemeProvider, unstable_createMuiStrictModeTheme } from '@mui/material/styles';
import { useNavigate, useParams } from 'react-router-dom';
import * as assetService from '../../services/assets'
import * as categoryService from '../../services/category'
import CategorySelectBoxDisabled from '../../components/custom/CategorySelectBoxDisabled'
import RadioButtonGroup from '../../components/custom/RadioButtonGroupInput'
import AssetEditToolbar from '../../components/formToolbar/AssetEditToolbar'


var today = new Date();
var dd = String(today.getDate()).padStart(2, '0');
var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
var yyyy = String(today.getFullYear());
var currentDay = yyyy + '-' + mm + '-' + dd

function EditAssetInformations() {
    const [category, setCategory] = useState([])
    const [isValid, setIsValid] = useState(true);
    const { id } = useParams();
    const [asset, setAsset] = useState({
        name: null,
        specification: null,
        installedDate: null,
        state: -1,
        categoryId: 0
    });
    const navigate = useNavigate();
    let theme = createTheme();
    theme = unstable_createMuiStrictModeTheme(theme);
    // console.log(asset)
    useEffect(() => {
        categoryService.getCategory()
            .then(responseData => setCategory(responseData))
            .catch(error => console.log(error))
        assetService.getAssetById(id)
            .then(responseData => {
                setAsset(responseData)
            })
            .catch(error => console.log(error))
    }, [])
    console.log("asset", asset);

    return (
        <ThemeProvider theme={theme}>
            <Container component="main">
                <CssBaseline />
                <Box
                    sx={{
                        margin: "auto",
                        marginTop: 8,
                        display: 'flex',
                        flexDirection: 'column',
                        // alignItems: 'center',
                    }}
                >
                    <Typography component="h3" variant="h5" color="#cf2338" pb="40px" fontWeight="bold">
                        Create New Asset
                    </Typography>
                    <Edit sx={{ ".RaEdit-card": { boxShadow: "none" }, ".RaEdit-main": { width: '750px' } }} mutationMode="pessimistic">
                        <SimpleForm toolbar={<AssetEditToolbar />} >
                            <Box
                                sx={{ display: "flex", flexDirection: "row", width: "650px", marginTop: "10px" }}
                            >
                                <Typography
                                    variant="h6"
                                    style={{
                                        width: "220px",
                                        margin: "0",
                                        padding: "0",
                                        alignSelf: "center"
                                    }}
                                >Name *</Typography>
                                <TextInput
                                    fullWidth
                                    label={false}
                                    name="name"
                                    source="name"
                                    style={{ width: "430px", margin: "0", padding: "0" }}
                                    helperText={false}
                                    InputLabelProps={{ shrink: false }}
                                />
                            </Box>
                            <Box
                                style={{ display: "flex", flexDirection: "row", width: "650px", marginTop: "10px" }}>
                                <Typography
                                    variant="h6"
                                    style={{
                                        width: "220px",
                                        margin: "0",
                                        padding: "0",
                                        alignSelf: "center",

                                    }}
                                >Installed Date *</Typography>
                                <DateInput
                                    fullWidth
                                    label=""
                                    name="installedDate"
                                    source="installedDate"
                                    InputLabelProps={{ shrink: false }}
                                    onBlur={(e) => e.stopPropagation()}
                                    style={{ width: "430px" }}
                                    helperText={false}
                                />
                            </Box>
                            <Box
                                style={{ display: "flex", flexDirection: "row", width: "650px", marginTop: "10px" }}
                            >
                                <Typography
                                    variant="h6"
                                    style={{
                                        width: "220px",
                                        margin: "0",
                                        padding: "0",
                                        alignSelf: "center"
                                    }}
                                >Category *</Typography>
                                <CategorySelectBoxDisabled
                                    source="category"
                                    format={(formValue) => (Array.prototype.filter.bind(category)(item => item.id === formValue))["name"]}
                                    parse=""
                                    defaultValue={asset.categoryId}
                                    disabled={true}
                                />
                            </Box>

                            <Box
                                style={{ display: "flex", flexDirection: "row", width: "650px", marginTop: "10px" }}
                            >
                                <Typography
                                    variant="h6"
                                    style={{
                                        width: "220px",
                                        margin: "0",
                                        padding: "0",
                                        alignSelf: "center",
                                        marginTop: "3px"
                                    }}
                                >Specification *</Typography>
                                <TextInput
                                    label={false}
                                    fullWidth
                                    multiline
                                    rows="3"
                                    style={{ width: "430px" }}
                                    name="specification"
                                    source="specification"
                                    helperText={false}
                                    defaultValue={asset.specification}
                                    InputLabelProps={{ shrink: false }}
                                />
                            </Box>
                            <Box
                                style={{ display: "flex", flexDirection: "row", width: "650px", marginTop: "10px" }}
                            >
                                <Typography
                                    variant="h6"
                                    style={{
                                        width: "220px",
                                        margin: "0",
                                        padding: "0",
                                        alignSelf: "start",
                                        paddingTop: "10px"
                                    }}
                                >State *</Typography>
                                <RadioButtonGroup label="" row={false} style={{ width: "430px" }} source="state" choices={[
                                    { id: 0, name: "Available" },
                                    { id: 1, name: "Not available" },
                                    { id: 2, name: "WaitingForRecycling" },
                                    { id: 3, name: "Recycled" }]}
                                    optionText="name"
                                    optionValue="id" />
                            </Box>
                        </SimpleForm>
                    </Edit>
                </Box>
            </Container>
        </ThemeProvider>
    )
}

export default EditAssetInformations;